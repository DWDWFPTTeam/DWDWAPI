using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DWDW_API.Core.Infrastructure;
using DWDW_API.Providers;
using DWDW_Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DWDW_API.Controllers
{
    [Route("api/[controller]")]
    public class RecordController : BaseController
    {
        private readonly IRecordService recordService;
        public RecordController(ExtensionSettings extensionSettings, IRecordService recordService) : base(extensionSettings)
        {
            this.recordService = recordService;
        }

        [Route("SaveRecord")]
        [AllowAnonymous]
        [HttpPost]
        public IActionResult SaveRecord(int deviceId, string image)
        {
            IActionResult result;
            try
            {
                var record = recordService.SaveRecord(deviceId, image);
                return Ok(record);
            }
            catch (BaseException e)
            {
                result = BadRequest(e.Message);
            }
            catch (Exception e)
            {

                result = StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            return result;
        }
        
    }
}