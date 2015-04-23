using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwilioRegistration.DataTypes;

namespace TwilioRegistration.BusinessLogic.Data
{
    public class HumanAccount : Account
    {
        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(255)]
        public string LastName { get; set; }

        [Index(IsUnique = true)]
        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        public override string Username
        {
            get
            {
                return Email;
            }
            set
            {
                base.Username = value;
                Email = value;
            }
        }

        [Index(IsUnique = true)]
        [Required]
        [MaxLength(25)]
        public string Prefix { get; set; }

        public virtual ICollection<Device> Devices { get; set; }

        public override AccountDT GetConcreteDT()
        {
            var account = new HumanAccountDT();
            account.FirstName = FirstName;
            account.LastName = LastName;
            account.Email = Email;
            account.Prefix = Prefix;
            return base.GetConcreteDT();
        }
    }
}
