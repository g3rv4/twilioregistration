using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwilioRegistration.DataTypes.Enums;

namespace TwilioRegistration.DataTypes
{
    public class DeviceDT
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public DeviceStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
