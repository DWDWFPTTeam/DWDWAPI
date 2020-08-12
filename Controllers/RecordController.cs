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
        [Route("GetRecordsByLocationDate")]
        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        public IActionResult GetRecordsByLocationDate([FromQuery]LocationRecordReceiveDateModel model)
        {
            return ExecuteInMonitoring(() =>
            {
                return recordService.GetRecordsByLocationDate(model.LocationId, model.startDate, model.endDate);
            });
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