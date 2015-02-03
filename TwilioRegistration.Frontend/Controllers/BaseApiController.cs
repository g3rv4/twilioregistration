using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                int accountId;
                if (int.TryParse(User.Identity.Name, out accountId))
                {
                    return accountId;
                }
                throw new InvalidOperationException();
            }
        }
    }
}
