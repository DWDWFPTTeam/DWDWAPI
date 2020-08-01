﻿using DWDW_API.Core.Constants;
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
using System.Security.Claims;

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
        public IActionResult SaveRecord(RecordReceivedModel recordReceived)
        {
            IActionResult result;
            try
            {
                var recordViewModel = recordService.SaveRecord(recordReceived);
                result = Ok(recordViewModel);
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

        //ham de test
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
                result = BadRequest(e.Message);
            }
            catch (Exception e)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            return result;
        }

        [Route("GetRecordByWorkerDate")]
        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        public IActionResult GetRecordByWorkerDate(int workerID, DateTime date)
        {
            IActionResult result;
            try
            {
                var record = recordService.GetRecordByWorkerDate(workerID, date);
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

        [Route("GetRecordByWorkerDateForManager")]
        [Authorize(Roles = Constant.MANAGER)]
        [HttpGet]
        public IActionResult GetRecordByWorkerDateForManager(int workerID, DateTime date)
        {
            IActionResult result;
            var identity = (ClaimsIdentity)User.Identity;
            var ID = (identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            int userID = int.Parse(ID);
            try
            {
                var record = recordService.GetRecordByWorkerDateForManager(userID,workerID, date);
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