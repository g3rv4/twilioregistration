using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using TwilioRegistration.BusinessLogic.Managers;

namespace TwilioRegistration.Frontend.Utils
{
    public class TokenValidationAttribute : IAuthenticationFilter
    {
        public async System.Threading.Tasks.Task AuthenticateAsync(HttpAuthenticationContext context, System.Threading.CancellationToken cancellationToken)
        {
            // Even if a token is valid from the OAuth perspective, I want to verify that's still active
            var identity = (ClaimsIdentity)context.Principal.Identity;
            var claim = identity.Claims.Where(c => c.Type == "token").FirstOrDefault();
            if (claim != null)
            {
                var realAccountId = AccountsMgr.GetAccountId(claim.Value);
                if (realAccountId.HasValue)
                {
                    var accountId = realAccountId.Value;

                    identity.AddClaim(new Claim("realAccountId", accountId.ToString()));
                    
                    if (context.Request.Headers.Any(h => h.Key == "Acting-As"))
                    {
                        var actingAs = context.Request.Headers.GetValues("Acting-As").FirstOrDefault();
                        if (actingAs != null && identity.HasClaim("permission", "act-as"))
                        {
                            if (int.TryParse(actingAs, out accountId))
                            {
                                //refresh the roles and permissions with the user's
                                foreach (var currentClaim in identity.Claims.Where(c => c.Type == "role" || c.Type == "permission").ToList())
                                {
                                    identity.RemoveClaim(currentClaim);
                                }
                                identity.AddClaims(AccountsMgr.GetRoles(accountId).Select(r => new Claim("role", r)));
                                identity.AddClaims(AccountsMgr.GetPermissions(accountId).Select(p => new Claim("permission", p)));
                            }
                        }
                    }

                    identity.AddClaim(new Claim("accountId", accountId.ToString()));
                }
                else
                {
                    context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
                }
            }
        }

        public async System.Threading.Tasks.Task ChallengeAsync(HttpAuthenticationChallengeContext context, System.Threading.CancellationToken cancellationToken)
        {
        }

        public bool AllowMultiple
        {
            get { throw new NotImplementedException(); }
        }
    }
}