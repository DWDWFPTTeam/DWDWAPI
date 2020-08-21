using DWDW_API.Core.Constants;
using DWDW_API.Core.ViewModels;
using DWDW_API.Providers;
using DWDW_Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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


        //[Route("SaveRecordByte")]
        //[AllowAnonymous]
        //[HttpPost]
        //public async Task<dynamic> SaveRecordByte([FromBody]RecordReceivedByteModel recordReceived)
        //{

        //    return await ExecuteInMonitoringAsync(async () =>
        //    {
        //        return await recordService.SaveRecordByte(recordReceived, this.ImageRoot);
        //    });
        //}

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
        public dynamic GetRecordsByLocationDate([FromQuery]RecordLocationReceivedViewModel record)
        {
            return ExecuteInMonitoring(() =>
            {
                return recordService.GetRecordsByLocationDate(record);
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

        [Route("GetRecordByLocationWorkerDateForManager")]
        [Authorize(Roles = Constant.MANAGER)]
        [HttpGet]
        public dynamic GetRecordByLocationWorkerDateForManager(int workerID,int locationID, DateTime date)
        {
            int userID = int.Parse(CurrentUserId);
            return ExecuteInMonitoring(() =>
            {
                return recordService.GetRecordByLocationWorkerDateForManager(userID, workerID, locationID, date);
            });
        }

        [Route("GetRecordByLocationDateForWorker")]
        [Authorize(Roles = Constant.WORKER)]
        [HttpGet]
        public dynamic GetRecordByLocationDateForWorker(int locationID, DateTime date)
        {
            int userID = int.Parse(CurrentUserId);
            return ExecuteInMonitoring(() =>
            {
                return recordService.GetRecordByLocationDateForWorker(userID, locationID, date);
            });
        }

    }
}