using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwilioRegistration.BusinessLogic.Data;
using TwilioRegistration.DataTypes;
using System.Data.Entity;
using System.Text.RegularExpressions;
using TwilioRegistration.DataTypes.Enums;
using TwilioRegistration.DataTypes.Exceptions;

namespace TwilioRegistration.BusinessLogic.Managers
{
    public static class DevicesMgr
    {
        public static async Task<IEnumerable<DeviceDT>> GetDevicesAsync(int accountId)
        {
            using (var context = new Context())
            {
                return (await context.Devices.
                    Where(d => d.Status != DeviceStatus.DELETED && d.AccountId == accountId).
                    OrderBy(d => d.Username).ToListAsync()
                    ).Select(d => d.GetDT()).ToList();
            }
        }

        public static async Task AddDeviceAsync(int accountId, string username, string password)
        {
            using (var context = new Context())
            {
                if (!Regex.IsMatch(username, "^[a-z0-9]+$", RegexOptions.IgnoreCase))
                {
                    throw new InvalidUsernameException();
                }
                if (password.Length < 8)
                {
                    throw new InvalidPasswordException();
                }
                var account = await AccountsMgr.GetAccountAsync(accountId, context);
                if (account.Devices.Any(d => d.Username == username))
                {
                    throw new UsernameTakenException();
                }

                var device = new Device();
                device.Account = account;
                device.Username = username;
                device.Password = password;
                device.Status = DeviceStatus.PENDING_CHANGES;
                context.Devices.Add(device);
                await context.SaveChangesAsync();
            }
        }

        public static async Task DeleteDeviceAsync(int accountId, int deviceId)
        {
            using (var context = new Context())
            {
                var account = await AccountsMgr.GetAccountAsync(accountId, context);
                var device = account.Devices.Where(d => d.Id == deviceId && d.Status != DeviceStatus.DELETED).FirstOrDefault();
                if (device == null)
                {
                    throw new InvalidDeviceException();
                }
                device.Status = DeviceStatus.DELETED;
                await context.SaveChangesAsync();
            }
        }
    }
}
