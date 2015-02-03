using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace TwilioRegistration.Frontend.Utils
{
    public class ClaimsAuthorizeAttribute : AuthorizeAttribute
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public ClaimsAuthorizeAttribute() { }

        public ClaimsAuthorizeAttribute(string claimType, string claimValue)
        {
            ClaimType = claimType;
            ClaimValue = claimValue;
        }

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var user = HttpContext.Current.User as ClaimsPrincipal;

            // if ClaimType is null, just validate that the user is logged in
            if (ClaimType == null && user.Claims.Any())
            {
                base.OnAuthorization(actionContext);
            }
            else if (ClaimType != null && user.HasClaim(ClaimType, ClaimValue))
            {
                base.OnAuthorization(actionContext);
            }
            else
            {
                base.HandleUnauthorizedRequest(actionContext);
            }
        }
    }
}