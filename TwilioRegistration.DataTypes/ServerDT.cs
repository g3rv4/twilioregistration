using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwilioRegistration.DataTypes
{
    public class ServerDT
    {
        public int Id { get; set; }

        public string Ip { get; set; }

        public bool AcceptsRegistrations { get; set; }
    }
}
