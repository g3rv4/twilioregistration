using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwilioRegistration.DataTypes.Enums
{
    public enum LogInStatus
    {
        SUCCESS,
        INVALID_USER_PWD,
        INACTIVE,
        TEMPORARILY_DISABLED
    }
}
