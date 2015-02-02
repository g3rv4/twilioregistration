using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwilioRegistration.BusinessLogic.Helpers;
using TwilioRegistration.DataTypes;

namespace TwilioRegistration.BusinessLogic.Data
{
    public class Account
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(255)]
        public string LastName { get; set; }

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

        public virtual Server Server { get; set; }

        public virtual ICollection<Device> Devices { get; set; }

        public bool PasswordMatches(string pwd)
        {
            return PasswordHasher.ValidatePassword(pwd, HashedPassword);
        }

        public virtual ICollection<Role> Roles { get; set; }

        public AccountDT GetDT(bool includeRoles = false)
        {
            return new AccountDT()
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Prefix = Prefix,
                CreatedAt = CreatedAt,
                IsActive = IsActive,
                Server = Server.GetDT(),
                Roles = includeRoles ? Roles.Select(r=>r.Name).ToList() : new List<string>()
            };
        }
    }
}
