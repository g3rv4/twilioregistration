using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TwilioRegistration.BusinessLogic.Managers;
using TwilioRegistration.DataTypes;
using TwilioRegistration.Frontend.Models.Accounts;

namespace TwilioRegistration.Frontend.Controllers
{
    public class AccountsController : BaseApiController
    {
        [Authorize]
        public IEnumerable<AccountDT> Get()
        {
            return AccountsMgr.GetAccountsForUser(_AccountId);
        }

        [HttpPost]
        [ActionName("log-in")]
        public LogInResultDT LogIn([FromBody]LogInVM data)
        {
            return AccountsMgr.LogIn(data.Email, data.Password);
        }
    }
}
