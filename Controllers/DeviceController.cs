using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DWDW_API.Providers;
using DWDW_Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DWDW_API.Controllers
{
    [Route("api/[controller]")]
    public class DeviceController : BaseController
    {
        private readonly IDeviceService deviceService;
        public DeviceController(ExtensionSettings extensionSettings, IDeviceService deviceService) : base(extensionSettings)
        {
            this.deviceService = deviceService;
        }

    }
}