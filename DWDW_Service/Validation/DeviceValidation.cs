using DWDW_Service.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_Service.Validation
{
    public class DeviceValidation
    {
        private readonly IDeviceRepository devicerepository;

        public DeviceValidation(IDeviceRepository devicerepository)
        {
            this.devicerepository = devicerepository;
        }
    }
}
