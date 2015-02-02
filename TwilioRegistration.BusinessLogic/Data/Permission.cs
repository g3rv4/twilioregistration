using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwilioRegistration.BusinessLogic.Data
{
    public class Permission
    {
        public int Id { get; set; }

        [Index(IsUnique = true)]
        [MaxLength(255)]
        public string CodeName { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
