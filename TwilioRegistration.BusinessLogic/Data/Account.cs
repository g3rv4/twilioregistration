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

        [Index(IsUnique = true)]
        [Required]
        [MaxLength(255)]
        public virtual string Username { get; set; }

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

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

        public int? ServerId { get; set; }

        public virtual Server Server { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public bool PasswordMatches(string pwd)
        {
            return PasswordHasher.ValidatePassword(pwd, HashedPassword);
        }

        public AccountDT GetDT()
        {
            var account = GetConcreteDT();
            account.Id = Id;
            account.CreatedAt = CreatedAt;
            account.IsActive = IsActive;
            if (Server != null)
            {
                account.Server = Server.GetDT();
            }
            return account;
        }

        public virtual AccountDT GetConcreteDT()
        {
            return new AccountDT();
        }
    }
}
