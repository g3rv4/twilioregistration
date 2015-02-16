using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using TwilioRegistration.DataTypes.Exceptions;

namespace TwilioRegistration.Frontend.Utils
{
    public class BusinessExceptionsFilter : ExceptionFilterAttribute 
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is BusinessException)
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionExecutedContext.Exception.GetType().ToString());
            }
        }
    }
}