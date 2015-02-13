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
            var loginResult = await AccountsMgr.LogInAsync(context.UserName, context.Password);
            if (loginResult.Status == LogInResult.SUCCESS)
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType, "accountId", "role");
                identity.AddClaim(new Claim("token", loginResult.Token));
                identity.AddClaims((await AccountsMgr.GetRolesAsync(loginResult.AccountId.Value)).Select(r => new Claim("role", r)));
                identity.AddClaims((await AccountsMgr.GetPermissionsAsync(loginResult.AccountId.Value)).Select(p => new Claim("permission", p)));
                context.Validated(identity);
            }
            else
            {
                context.SetError(loginResult.Status.ToString());
            }
        }
    }
}