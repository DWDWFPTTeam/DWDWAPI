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
    public class ShiftController : BaseController
    {
        private readonly IShiftService shiftService;
        public ShiftController(ExtensionSettings extensionSettings, IShiftService shiftService) : base(extensionSettings)
        {
            this.shiftService = shiftService;
        }
    }
}