using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using TwilioRegistration.BusinessLogic.Data;
using TwilioRegistration.BusinessLogic.Helpers;
using TwilioRegistration.DataTypes;
using TwilioRegistration.DataTypes.Enums;
using StackExchange.Redis;
using TwilioRegistration.DataTypes.Exceptions;

namespace TwilioRegistration.BusinessLogic.Managers
{
    public static class AccountsMgr
    {
        public static async Task<Tuple<Guid, int>> LogInAsync(string username, string password)
        {
            using (var context = new Context())
            {
                // doing the temporary lock of accounts on redis, so that we even lock non existent ones. This lets us give real users useful feedback, while an attacker would get the temporarily disabled message for every account (not being able to find out which ones exist and which ones don't)
                int maxFailedLogins = int.Parse(ConfigurationManager.AppSettings["Account.MaxFailedLogins"]);

                int failedLogins = 0;
                int.TryParse(await RedisConnection.Instance.Database.StringGetAsync("failed-logins:" + username), out failedLogins);

                if (failedLogins >= maxFailedLogins)
                {
                    throw new AccountTemporarilyDisabledException();
                }
                
                var account = await context.Accounts.Where(a => a.Username == username).FirstOrDefaultAsync();
                if (account != null && account.PasswordMatches(password))
                {
                    // verify if the account is active once we know that the user knows their pwd and that their account isn't temporarily disabled
                    if (!account.IsActive)
                    {
                        throw new AccountInactiveException();
                    }

                    var token = Guid.NewGuid();

                    int tokenDeactivationSeconds = int.Parse(ConfigurationManager.AppSettings["Account.TokenExpirationSeconds"]);
                    RedisConnection.Instance.Database.StringSet("token:" + token.ToString(), account.Id, TimeSpan.FromSeconds(tokenDeactivationSeconds), flags: CommandFlags.FireAndForget);

                    return new Tuple<Guid,int>(token, account.Id);
                }
                else
                {
                    int accountDeactivationSeconds = int.Parse(ConfigurationManager.AppSettings["Account.AccountDeactivationSeconds"]);
                    RedisConnection.Instance.Database.StringSet("failed-logins:" + username, failedLogins + 1, TimeSpan.FromSeconds(accountDeactivationSeconds), flags: CommandFlags.FireAndForget);
                }
            }
            throw new InvalidUsernameOrPasswordException();
        }

        public static async Task<int?> GetAccountIdAsync(string token)
        {
            int accountId;
            if (int.TryParse(RedisConnection.Instance.Database.StringGet("token:" + token), out accountId))
            {
                int tokenDeactivationSeconds = int.Parse(ConfigurationManager.AppSettings["Account.TokenExpirationSeconds"]);
                await RedisConnection.Instance.Database.KeyExpireAsync("token:" + token, TimeSpan.FromSeconds(tokenDeactivationSeconds));
                return accountId;
            }
            return null;
        }

        public static async Task<AccountDT> GetAccountAsync(int accountId, bool onlyActive = true)
        {
            using (var context = new Context())
            {
                return (await GetAccountAsync(accountId, context, onlyActive)).GetDT();
            }
        }

        internal static async Task<Account> GetAccountAsync(int accountId, Context context, bool onlyActive = true, bool onlyHuman = false)
        {
            var query = context.Accounts.Where(a => a.Id == accountId);
            if (onlyActive)
            {
                query = query.Where(a => a.IsActive);
            }
            var account = await query.FirstOrDefaultAsync();
            if (account == null || (onlyHuman && !(account is HumanAccount)))
            {
                throw new InvalidAccountException();
            }
            return account;
        }

        public static async Task<List<string>> GetRolesAsync(int accountId)
        {
            using (var context = new Context())
            {
                return await context.Roles.Where(r => r.Accounts.Any(a => a.Id == accountId)).Select(r => r.Name).ToListAsync();
            }
        }

        public static async Task<List<string>> GetPermissionsAsync(int accountId)
        {
            using (var context = new Context())
            {
                return await context.Permissions.Where(p => p.Roles.Any(r => r.Accounts.Any(a => a.Id == accountId))).Select(p => p.CodeName).ToListAsync();
            }
        }

        public static async Task<IEnumerable<AccountDT>> GetAccountsAsync()
        {
            using (var context = new Context())
            {
                return (await context.Accounts.Where(a => a.IsActive).ToListAsync()).Select(a => a.GetDT()).ToList();
            }
        }
    }
}
