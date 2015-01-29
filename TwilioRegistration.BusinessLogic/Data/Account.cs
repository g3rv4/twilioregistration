using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwilioRegistration.BusinessLogic.Helpers;

namespace TwilioRegistration.BusinessLogic.Data
{
    internal class Account
    {
        public int Id { get; set; }

        [Index(IsUnique=true)]
        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [NotMapped]
        public string Password
        {
            set
            {
                HashedPassword = PasswordHasher.CreateHash(value);
            }
        }

        [Required]
        public string HashedPassword { get; set; }

        [Index(IsUnique = true)]
        [Required]
        [MaxLength(25)]
        public string Prefix { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

        public int ServerId { get; set; }

        public Server Server { get; set; }

        public List<Device> Devices { get; set; }

        public bool PasswordMatches(string pwd)
        {
            return PasswordHasher.ValidatePassword(pwd, HashedPassword);
        }
    }
}
