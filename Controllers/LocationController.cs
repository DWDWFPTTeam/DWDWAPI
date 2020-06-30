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
    [ApiController]
    public class LocationController : BaseController
    {
        private readonly ILocationService locationService;
        public LocationController(ExtensionSettings extensionSettings, ILocationService locationService) : base(extensionSettings)
        {
            this.locationService = locationService;
        }
    }
}