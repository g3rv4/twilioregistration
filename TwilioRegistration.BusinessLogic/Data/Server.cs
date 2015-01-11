using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwilioRegistration.BusinessLogic.Data
{
    internal class Server
    {
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [MaxLength(15)]
        public string Ip { get; set; }

        public List<Account> Accounts { get; set; }

        public bool AcceptsRegistrations { get; set; }
    }
}
