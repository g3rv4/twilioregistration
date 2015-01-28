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
                var account = context.Accounts.Where(a => a.Email == email).FirstOrDefault();
                if (account != null)
                {
                    if (!account.IsActive)
                    {
                        res.Status = LogInStatus.INACTIVE;
                        return res;
                    }
                    if (account.ReactivationTime.HasValue)
                    {
                        if (account.ReactivationTime.Value < DateTime.UtcNow)
                        {
                            account.ReactivationTime = null;
                            account.FailedLoginAttempts = 0;
                        }
                        else
                        {
                            res.Status = LogInStatus.TEMPORARILY_DISABLED;
                            return res;
                        }
                    }
                    if (account.PasswordMatches(password))
                    {
                        res.Status = LogInStatus.SUCCESS;
                        res.Token = System.Guid.NewGuid().ToString();

                        RedisConnection.Instance.Database.StringSetAsync(res.Token, account.Id, TimeSpan.FromSeconds(int.Parse(ConfigurationManager.AppSettings["Account.TokenExpirationSeconds"])));

                        account.FailedLoginAttempts = 0;
                    }
                    else
                    {
                        account.FailedLoginAttempts++;
                        int maxFailedLogins = int.Parse(ConfigurationManager.AppSettings["Account.MaxFailedLogins"]);
                        if (account.FailedLoginAttempts >= maxFailedLogins)
                        {
                            int deactivateSeconds = int.Parse(ConfigurationManager.AppSettings["Account.AccountDeactivationSeconds"]);
                            account.ReactivationTime = DateTime.UtcNow.AddSeconds(deactivateSeconds);
                        }
                    }
                    context.SaveChanges();
                }
            }
            return res;
        }
    }
}
