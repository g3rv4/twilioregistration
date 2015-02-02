﻿using System;
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

        public string Email { get; set; }

        public string Prefix { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

        public ServerDT Server { get; set; }

        public List<string> Roles { get; set; }
    }
}