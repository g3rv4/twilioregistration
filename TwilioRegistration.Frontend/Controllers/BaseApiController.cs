using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using TwilioRegistration.BusinessLogic.Managers;

namespace TwilioRegistration.Frontend.Controllers
{
    public class BaseApiController : ApiController
    {
        protected int _AccountId
        {
            get
            {
                string token = ((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == "token").FirstOrDefault().Value;
                if (token != null)
                {
                    var accId = AccountsMgr.GetAccountId(token);
                    if (accId != null)
                    {
                        var accountId = accId.Value;
                        // let some users act as another ones (useful for reproducing issues)
                        if (Request.Headers.Any(h => h.Key == "Acting-As"))
                        {
                            var actingAs = Request.Headers.GetValues("Acting-As").FirstOrDefault();
                            if (actingAs != null && AccountsMgr.HasPermission(accountId, "act-as"))
                            {
                                int.TryParse(actingAs, out accountId);
                            }
                        }
                        return accountId;
                    }
                }
                throw new InvalidOperationException();
            }
        }
    }
}
