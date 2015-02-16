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
            try
            {
                var loginResult = await AccountsMgr.LogInAsync(context.UserName, context.Password);
                var identity = new ClaimsIdentity(context.Options.AuthenticationType, "accountId", "role");
                identity.AddClaim(new Claim("token", loginResult.Item1.ToString()));
                identity.AddClaims((await AccountsMgr.GetRolesAsync(loginResult.Item2)).Select(r => new Claim("role", r)));
                identity.AddClaims((await AccountsMgr.GetPermissionsAsync(loginResult.Item2)).Select(p => new Claim("permission", p)));
                context.Validated(identity);
            }
            catch (Exception ex)
            {
                context.SetError(ex.GetType().ToString());
            }
        }
    }
}