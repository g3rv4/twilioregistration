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

            string token = ((ClaimsIdentity)context.Principal.Identity).Claims.Where(c => c.Type == "token").FirstOrDefault().Value;
            if (token != null && !AccountsMgr.GetAccountId(token).HasValue)
            {
                context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
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