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
    }
}
