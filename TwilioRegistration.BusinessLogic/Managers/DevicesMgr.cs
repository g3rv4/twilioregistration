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
using TwilioRegistration.DataTypes.Enums.Results;

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

        public static async Task<AddDevice> AddDeviceAsync(int accountId, string username, string password)
        {
            using (var context = new Context())
            {
                if (!Regex.IsMatch(username, "^[a-z0-9]+$", RegexOptions.IgnoreCase))
                {
                    return AddDevice.INVALID_USERNAME;
                }
                if (password.Length < 8)
                {
                    return AddDevice.INVALID_PASSWORD;
                }
                var account = await AccountsMgr.GetAccountAsync(accountId, context);
                if (account == null)
                {
                    return AddDevice.INVALID_ACCOUNT;
                }
                if (account.Devices.Any(d => d.Username == username))
                {
                    return AddDevice.USERNAME_TAKEN;
                }

                var device = new Device();
                device.Account = account;
                device.Username = username;
                device.Password = password;
                device.Status = DeviceStatus.PENDING_CHANGES;
                context.Devices.Add(device);
                await context.SaveChangesAsync();

                return AddDevice.SUCCESS;
            }
        }

        public static async Task<DeleteDevice> DeleteDeviceAsync(int accountId, int deviceId)
        {
            using (var context = new Context())
            {
                var account = await AccountsMgr.GetAccountAsync(accountId, context);
                if (account == null)
                {
                    return DeleteDevice.INVALID_ACCOUNT;
                }
                var device = account.Devices.Where(d => d.Id == deviceId && d.Status != DeviceStatus.DELETED).FirstOrDefault();
                if (device == null)
                {
                    return DeleteDevice.INVALID_DEVICE;
                }
                device.Status = DeviceStatus.DELETED;
                await context.SaveChangesAsync();
                return DeleteDevice.SUCCESS;
            }
        }
    }
}
