using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using TwilioRegistration.BusinessLogic.Managers;
using TwilioRegistration.DataTypes.Enums;

namespace TwilioRegistration.Frontend.Utils
{
    public class TWRAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var loginResult = AccountsMgr.LogIn(context.UserName, context.Password);
            if (loginResult.Status == LogInStatus.SUCCESS)
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType, "accountId", "role");
                identity.AddClaim(new Claim("token", loginResult.Token));
                identity.AddClaims(AccountsMgr.GetRoles(loginResult.AccountId.Value).Select(r => new Claim("role", r)));
                context.Validated(identity);
            }
            else
            {
                context.SetError(loginResult.Status.ToString());
            }
        }
    }
}