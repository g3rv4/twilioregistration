using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        public async Task<IEnumerable<AccountDT>> Get()
        {
            return await AccountsMgr.GetAccountsAsync();
        }

        [HttpGet]
        [Route("current")]
        public async Task<AccountDT> CurrentAccountId()
        {
            return await AccountsMgr.GetAccountAsync(_AccountId);
        }
    }
}
