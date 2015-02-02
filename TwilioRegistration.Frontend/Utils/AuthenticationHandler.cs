using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using TwilioRegistration.BusinessLogic.Managers;

namespace TwilioRegistration.Frontend.Utils
{
    public class AuthenticationHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authHeader = request.Headers.Authorization;

            if (authHeader == null)
            {
                return base.SendAsync(request, cancellationToken);
            }

            if (authHeader.Scheme != "gmc-auth")
            {
                return base.SendAsync(request, cancellationToken);
            }

            var account = AccountsMgr.GetAccount(authHeader.Parameter.Trim());
            if (account == null)
            {
                return base.SendAsync(request, cancellationToken);
            }

            var identity = new GenericIdentity(account.Id.ToString(), "gmc-auth");
            string[] roles = Roles.Provider.GetRolesForUser(account.Id.ToString());
            var principal = new GenericPrincipal(identity, roles);
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}