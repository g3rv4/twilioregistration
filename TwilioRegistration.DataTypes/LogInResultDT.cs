using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwilioRegistration.DataTypes.Enums;

namespace TwilioRegistration.DataTypes
{
    public class LogInResultDT
    {
        public LogInResult Status { get; set; }

        public string Token { get; set; }

        public int? AccountId { get; set; }
    }
}