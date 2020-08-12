using DWDW_API.Core.Constants;
using DWDW_API.Core.Entities;
using DWDW_API.Core.Infrastructure;
using DWDW_API.Core.ViewModels;
using DWDW_API.Providers;
using DWDW_Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

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
        public async Task<dynamic> SaveRecord([FromForm]RecordReceivedModel recordReceived)
        {
           
            return await ExecuteInMonitoringAsync(async () =>
            {
                return await recordService.SaveRecord(recordReceived, this.ImageRoot);
            });
        }

        [Route("GetRecordById")]
        [Authorize(Roles = Constant.ADMIN + "," + Constant.MANAGER)]
        [HttpGet]
        public dynamic GetRecordById(int recordId)
        {
            return ExecuteInMonitoring(() =>
            {
                return recordService.GetRecordById(recordId);
            });
        }

        //ham de test
        [Route("GetRecordsByLocationId")]
        [AllowAnonymous]
        [HttpGet]
        public dynamic GetRecordsByLocationId(int locationId)
        {
            return ExecuteInMonitoring(() =>
            {
                return recordService.GetRecordByLocationId(locationId);
            });
        }
        [Route("GetRecordsByLocationIdAndTime/{locationId}/{start}")]
        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        public IActionResult GetRecordsByLocationIdAndTime(
            int locationId, string start, string end)
        {
            //chua check location exist
            IActionResult result;
            try
            {
                DateTime startDate;
                bool check = false;
                string pattern = "dd-MM-yyyy";
                check = DateTime.TryParseExact
                    (start, pattern, null, DateTimeStyles.None, out startDate);
                //check date is valid
                if (check == false) return BadRequest("Invalid date format");
                
                //chua check start-end cai nao phai lon hon

                IEnumerable record;
                if (!string.IsNullOrEmpty(end))
                {
                    //get weekly,monthly records
                    DateTime endDate;
                    check = DateTime.TryParseExact(end, pattern, null, DateTimeStyles.None, out endDate);
                    //check date is valid
                    if (check == false) return BadRequest();
                    record = recordService.GetRecordsByLocationIdBetweenTime(locationId, startDate, endDate);
                }
                else
                {
                    //get daily records
                    record = recordService.GetRecordsByLocationIdAndTime(locationId, startDate);
                }
                result = Ok(record);
            }
            catch (BaseException e)
            {
                result = BadRequest(new ErrorViewModel
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = e.Message
                });
            }
            catch (Exception e)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new ErrorViewModel
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = e.Message
                });
            }
            return result;
        }

        [Route("GetRecordByWorkerDate")]
        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        public dynamic GetRecordByWorkerDate(int workerID, DateTime date)
        {
            return ExecuteInMonitoring(() =>
            {
                return recordService.GetRecordByWorkerDate(workerID, date);
            });
        }

        [Route("GetRecordByWorkerDateForManager")]
        [Authorize(Roles = Constant.MANAGER)]
        [HttpGet]
        public dynamic GetRecordByWorkerDateForManager(int workerID, DateTime date)
        {
            int userID = int.Parse(CurrentUserId);
            return ExecuteInMonitoring(() =>
            {
                return recordService.GetRecordByWorkerDateForManager(userID, workerID, date);
            });
        }

    }
}