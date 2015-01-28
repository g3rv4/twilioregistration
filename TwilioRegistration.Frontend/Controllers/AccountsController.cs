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
    public class AccountsController : ApiController
    {
        // GET api/accounts/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/accounts
        public void Post([FromBody]string value)
        {
        }

        // PUT api/accounts/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/accounts/5
        public void Delete(int id)
        {
        }

        [HttpPost]
        [ActionName("log-in")]
        public LogInResultDT LogIn([FromBody]LogInVM data)
        {
            return AccountsMgr.LogIn(data.Email, data.Password);
        }
    }
}
