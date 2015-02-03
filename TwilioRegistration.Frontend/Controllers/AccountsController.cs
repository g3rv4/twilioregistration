using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TwilioRegistration.BusinessLogic.Managers;
using TwilioRegistration.DataTypes;
using TwilioRegistration.Frontend.Utils;

namespace TwilioRegistration.Frontend.Controllers
{
    [ClaimsAuthorizeAttribute]
    [RoutePrefix("api/accounts")]
    public class AccountsController : BaseApiController
    {
        [ClaimsAuthorize("permission", "view-all-accounts")]
        public IEnumerable<AccountDT> Get()
        {
            return AccountsMgr.GetAccounts();
        }

        [HttpGet]
        [Route("current")]
        public AccountDT CurrentAccountId()
        {
            return AccountsMgr.GetAccount(_AccountId);
        }
    }
}
