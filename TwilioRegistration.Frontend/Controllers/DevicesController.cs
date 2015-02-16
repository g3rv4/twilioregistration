using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TwilioRegistration.BusinessLogic.Managers;
using TwilioRegistration.DataTypes;
using TwilioRegistration.Frontend.Utils;

namespace TwilioRegistration.Frontend.Controllers
{
    [ClaimsAuthorize]
    public class DevicesController : BaseApiController
    {
        public async Task<IEnumerable<DeviceDT>> Get()
        {
            return await DevicesMgr.GetDevicesAsync(_AccountId);
        }

        public async Task Post([FromBody]DeviceDT device)
        {
            await DevicesMgr.AddDeviceAsync(_AccountId, device.Username, device.Password);
        }

        public async Task Delete(int id)
        {
            await DevicesMgr.DeleteDeviceAsync(_AccountId, id);
        }
    }
}
