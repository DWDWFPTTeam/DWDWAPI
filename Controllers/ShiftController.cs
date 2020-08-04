using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DWDW_API.Core.Constants;
using DWDW_API.Core.Infrastructure;
using DWDW_API.Core.ViewModels;
using DWDW_API.Providers;
using DWDW_Service.Services;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        [Route("GetAllShift")]
        public dynamic GetAllShift()
        {
            return ExecuteInMonitoring(() =>
            {
                return shiftService.GetAll();
            });
        }

        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        [Route("GetShiftByID")]
        public dynamic GetShiftByID(int id)
        {
            return ExecuteInMonitoring(() =>
            {
                return shiftService.GetByID(id);
            });
        }

        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        [Route("GetShiftByDate")]
        public dynamic GetShiftByDate(DateTime date)
        {
            return ExecuteInMonitoring(() =>
            {
                return shiftService.GetShiftByDate(date);
            });
        }

        [Authorize(Roles = Constant.MANAGER)]
        [HttpGet]
        [Route("GetShiftManager")]
        public dynamic GetShiftManager()
        {
            int userID = int.Parse(CurrentUserId);
            return ExecuteInMonitoring(() =>
            {
                return shiftService.GetShiftManager(userID);
            });
        }

        [Authorize(Roles = Constant.MANAGER)]
        [HttpGet]
        [Route("GetShiftByDateManageer")]
        public dynamic GetShiftByDateManager(DateTime date)
        {
            int userID = int.Parse(CurrentUserId);
            return ExecuteInMonitoring(() =>
            {
                return shiftService.GetShiftByDateManager(userID, date);
            });
        }

        [Authorize(Roles = Constant.WORKER)]
        [HttpGet]
        [Route("GetShiftWorker")]
        public dynamic GetShiftWorker()
        {
            int userID = int.Parse(CurrentUserId);
            return ExecuteInMonitoring(() =>
            {
                return shiftService.GetShiftWorker(userID);
            });
        }

        [Authorize(Roles = Constant.MANAGER)]
        [HttpPost]
        [Route("CreateShift")]
        public dynamic CreateShift(ShiftCreateModel shift)
        {
            int userID = int.Parse(CurrentUserId);
            return ExecuteInMonitoring(() =>
            {
                return shiftService.CreateShift(userID, shift);
            });
        }

        [Authorize(Roles = Constant.MANAGER)]
        [HttpPut]
        [Route("UpdateShift")]
        public dynamic UpdateShift( ShiftUpdateModel shift)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var ID = (identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            int userID = int.Parse(ID);
            return ExecuteInMonitoring(() =>
            {
                return shiftService.UpdateShift(userID, shift);
            });
        }
        [Authorize(Roles = Constant.MANAGER)]
        [HttpPut]
        [Route("UpdateShiftActive")]
        public dynamic UpdateShiftActive(ShiftActiveModel shift)
        {
            int userID = int.Parse(CurrentUserId);
            return ExecuteInMonitoring(() =>
            {
                return shiftService.UpdateShiftActive(userID, shift);
            });
        }

        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        [Route("GetShiftFromLocationByDate")]
        public dynamic GetShiftFromLocationByDate(int locationId, DateTime date)
        {
            return ExecuteInMonitoring(() =>
            {
                return shiftService.GetShiftFromLocationByDate(locationId, date);
            });
        }

        [Authorize(Roles = Constant.MANAGER)]
        [HttpGet]
        [Route("GetShiftFromLocationByDateManager")]
        public dynamic GetShiftFromLocationByDateManager(int locationId, DateTime date)
        {
            int userID = int.Parse(CurrentUserId);
            return ExecuteInMonitoring(() =>
            {
                return shiftService.GetShiftFromLocationByDateManager(userID, locationId, date);
            });
        }

        [Authorize(Roles = Constant.WORKER)]
        [HttpGet]
        [Route("GetShiftFromLocationByDateWorker")]
        public dynamic GetShiftFromLocationByDateWorker(int locationId, DateTime date)
        {
            int userID = int.Parse(CurrentUserId);
            return ExecuteInMonitoring(() =>
            {
                return shiftService.GetShiftFromLocationByDateWorker(userID, locationId, date);
            });
        }
    }
}