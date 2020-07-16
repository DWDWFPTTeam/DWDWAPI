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
using System.Globalization;

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
        [HttpGet]
        public IActionResult SaveRecord(string deviceCode, string image)
        {
            IActionResult result;
            try
            {
                var record = recordService.SaveRecord(deviceCode, image);
                result = Ok(record);
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

        [Route("GetRecordsByLocationId")]
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetRecordsByLocationId(int locationId)
        {
            IActionResult result;
            try
            {
                var record = recordService.GetRecordByLocationId(locationId);
                result = Ok(record);
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
        [Route("GetRecordsByLocationIdAndTime")]
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetRecordsByLocationIdAndTime(
            int locationId, string start, string end)
        {
            IActionResult result;
            try
            {
                DateTime startDate, endDate;
                bool check = false;
                string pattern = "dd/MM/yyyy";
                check = DateTime.TryParseExact(start, pattern, null, DateTimeStyles.None, out startDate);
                check = DateTime.TryParseExact(end, pattern, null, DateTimeStyles.None, out endDate);
                var record = recordService.GetRecordsByLocationIdBetweenTime(locationId, startDate, endDate);
                result = Ok(record);
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