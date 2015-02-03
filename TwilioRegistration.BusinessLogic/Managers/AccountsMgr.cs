using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwilioRegistration.BusinessLogic.Data;
using TwilioRegistration.BusinessLogic.Helpers;
using TwilioRegistration.DataTypes;
using TwilioRegistration.DataTypes.Enums;

namespace TwilioRegistration.BusinessLogic.Managers
{
    public static class AccountsMgr
    {
        public static LogInResultDT LogIn(string email, string password)
        {
            var res = new LogInResultDT() { Status = LogInStatus.INVALID_USER_PWD };
            using (var context = new Context())
            {
                // doing the temporary lock of accounts on redis, so that we even lock non existent ones. This lets us give real users useful feedback, while an attacker would get the temporarily disabled message for every account (not being able to find out which ones exist and which ones don't)
                int maxFailedLogins = int.Parse(ConfigurationManager.AppSettings["Account.MaxFailedLogins"]);

                int failedLogins = 0;
                int.TryParse(RedisConnection.Instance.Database.StringGet("failed-logins:" + email), out failedLogins);

                if (failedLogins >= maxFailedLogins)
                {
                    res.Status = LogInStatus.TEMPORARILY_DISABLED;
                    return res;
                }
                
                var account = context.Accounts.Where(a => a.Email == email).FirstOrDefault();
                if (account != null && account.PasswordMatches(password))
                {
                    // verify if the account is active once we know that the user knows their pwd and that their account isn't temporarily disabled
                    if (!account.IsActive)
                    {
                        res.Status = LogInStatus.INACTIVE;
                        return res;
                    }

                    res.Status = LogInStatus.SUCCESS;
                    res.Token = System.Guid.NewGuid().ToString();
                    res.AccountId = account.Id;

                    int tokenDeactivationSeconds = int.Parse(ConfigurationManager.AppSettings["Account.TokenExpirationSeconds"]);
                    RedisConnection.Instance.Database.StringSetAsync("token:" + res.Token, account.Id, TimeSpan.FromSeconds(tokenDeactivationSeconds));
                }
                else
                {
                    int accountDeactivationSeconds = int.Parse(ConfigurationManager.AppSettings["Account.AccountDeactivationSeconds"]);
                    RedisConnection.Instance.Database.StringSetAsync("failed-logins:" + email, failedLogins + 1, TimeSpan.FromSeconds(accountDeactivationSeconds));
                }
            }
            return res;
        }

        public static int? GetAccountId(string token)
        {
            int accountId;
            if (int.TryParse(RedisConnection.Instance.Database.StringGet("token:" + token), out accountId))
            {
                int tokenDeactivationSeconds = int.Parse(ConfigurationManager.AppSettings["Account.TokenExpirationSeconds"]);
                RedisConnection.Instance.Database.KeyExpire("token:" + token, TimeSpan.FromSeconds(tokenDeactivationSeconds));
                return accountId;
            }
            return null;
        }

        public static AccountDT GetAccount(int accountId)
        {
            using (var context = new Context())
            {
                var account = context.Accounts.Where(a => a.Id == accountId).FirstOrDefault();
                if (account != null)
                {
                    return account.GetDT();
                }
            }
            return null;
        }

        public static List<string> GetRoles(int accountId)
        {
            using (var context = new Context())
            {
                return context.Roles.Where(r => r.Accounts.Any(a => a.Id == accountId)).Select(r => r.Name).ToList();
            }
        }

        public static IEnumerable<AccountDT> GetAccountsForUser(int accountId)
        {
            using (var context = new Context())
            {
                var accounts = context.Accounts.Where(a => a.IsActive);
                if (!HasPermission(accountId, "view-all-accounts", context))
                {
                    accounts = accounts.Where(a => a.Id == accountId);
                }
                return accounts.ToList().Select(a => a.GetDT()).ToList();
            }
        }

        public static bool HasPermission(int accountId, string permission)
        {
            using (var context = new Context())
            {
                return HasPermission(accountId, permission, context);
            }
        }

        internal static bool HasPermission(int accountId, string permission, Context context)
        {
            return context.Accounts.Where(a => a.Id == accountId).Any(a => a.Roles.Any(r => r.Permissions.Any(p => p.CodeName == permission)));
        }
    }
}
