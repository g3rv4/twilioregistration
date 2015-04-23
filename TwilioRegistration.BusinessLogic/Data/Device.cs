using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TwilioRegistration.DataTypes;
using TwilioRegistration.DataTypes.Enums;

namespace TwilioRegistration.BusinessLogic.Data
{
    public class Device
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(25)]
        [Index("IX_UsernameAccount", 1, IsUnique = true)]
        public string Username { get; set; }

        [Required]
        [NotMapped]
        public string Password
        {
            set
            {
                HashedPassword = CalculateMD5Hash(this.Username + ":asterisk:" + value);
            }
        }

        [Required]
        public string HashedPassword { get; set; }

        [Required]
        public DeviceStatus Status { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [Required]
        [Index("IX_UsernameAccount", 2, IsUnique = true)]
        public int AccountId { get; set; }

        public virtual HumanAccount Account { get; set; }

        public DeviceDT GetDT()
        {
            return new DeviceDT()
            {
                Id = Id,
                Username = Username,
                Status = Status,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt
            };
        }

        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString().ToLower();
        }
    }
}
