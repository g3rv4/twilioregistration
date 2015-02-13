using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwilioRegistration.DataTypes.Enums.Results
{
    public enum AddDevice
    {
        SUCCESS,
        INVALID_ACCOUNT,
        INVALID_USERNAME,
        INVALID_PASSWORD,
        USERNAME_TAKEN
    }
}
