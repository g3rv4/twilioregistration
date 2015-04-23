using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwilioRegistration.DataTypes
{
    public class AccountDT
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

        public ServerDT Server { get; set; }
    }
}
