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
    public class Server
    {
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [MaxLength(15)]
        public string Ip { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }

        public bool AcceptsRegistrations { get; set; }

        public ServerDT GetDT()
        {
            return new ServerDT() { 
                Id = Id,
                Ip = Ip,
                AcceptsRegistrations = AcceptsRegistrations 
            };
        }
    }
}
