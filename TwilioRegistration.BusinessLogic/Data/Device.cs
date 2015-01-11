using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwilioRegistration.DataTypes.Enums;

namespace TwilioRegistration.BusinessLogic.Data
{
    internal class Device
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(25)]
        [Index("IX_UsernameAccount", 1, IsUnique = true)]
        public string Username { get; set; }

        public string Password { get; set; }

        public DeviceStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [Index("IX_UsernameAccount", 2, IsUnique = true)]
        public int AccountId { get; set; }

        public Account Account { get; set; }
    }
}
