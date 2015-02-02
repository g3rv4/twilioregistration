using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwilioRegistration.BusinessLogic.Data
{
    public class Role
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
