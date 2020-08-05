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
        public IActionResult GetAllShift()
        {
            IActionResult result;
            try
            {
                var shifts = shiftService.GetAll();
                result = Ok(shifts);
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

        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        [Route("GetShiftByID")]
        public IActionResult GetShiftByID(int id)
        {
            IActionResult result;
            try
            {
                var shifts = shiftService.GetByID(id);
                result = Ok(shifts);
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

        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        [Route("GetShiftByDate")]
        public IActionResult GetShiftByDate(DateTime date)
        {
            IActionResult result;
            try
            {
                var shifts = shiftService.GetShiftByDate(date);
                result = Ok(shifts);
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

        [Authorize(Roles = Constant.MANAGER)]
        [HttpGet]
        [Route("GetShiftManager")]
        public IActionResult GetShiftManager()
        {
            IActionResult result;
            var identity = (ClaimsIdentity)User.Identity;
            var ID = (identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            int userID = int.Parse(ID);
            try
            {
                var shifts = shiftService.GetShiftManager(userID);
                result = Ok(shifts);
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

        [Authorize(Roles = Constant.MANAGER)]
        [HttpGet]
        [Route("GetShiftByDateManageer")]
        public IActionResult GetShiftByDateManager(DateTime date)
        {
            IActionResult result;
            var identity = (ClaimsIdentity)User.Identity;
            var ID = (identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            int userID = int.Parse(ID);
            try
            {
                var shifts = shiftService.GetShiftByDateManager(userID, date);
                result = Ok(shifts);
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

        [Authorize(Roles = Constant.WORKER)]
        [HttpGet]
        [Route("GetShiftWorker")]
        public IActionResult GetShiftWorker()
        {
            IActionResult result;
            var identity = (ClaimsIdentity)User.Identity;
            var ID = (identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            int userID = int.Parse(ID);
            try
            {
                var shifts = shiftService.GetShiftWorker(userID);
                result = Ok(shifts);
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

        [Authorize(Roles = Constant.MANAGER)]
        [HttpPost]
        [Route("CreateShift")]
        public IActionResult CreateShift(int locationID, ShiftCreateModel shift)
        {
            IActionResult result;
            var identity = (ClaimsIdentity)User.Identity;
            var ID = (identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            int userID = int.Parse(ID);
            try
            {
                var shifts = shiftService.CreateShift(userID, locationID, shift);
                result = Ok(shifts);
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

        [Authorize(Roles = Constant.MANAGER)]
        [HttpPut]
        [Route("UpdateShift")]
        public IActionResult UpdateShift(int locationID, ShiftUpdateModel shift)
        {
            IActionResult result;
            var identity = (ClaimsIdentity)User.Identity;
            var ID = (identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            int userID = int.Parse(ID);
            try
            {
                var shifts = shiftService.UpdateShift(userID, locationID, shift);
                result = Ok(shifts);
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
        [Authorize(Roles = Constant.MANAGER)]
        [HttpPut]
        [Route("UpdateShiftActive")]
        public IActionResult UpdateShiftActive(ShiftActiveModel shift)
        {
            IActionResult result;
            var identity = (ClaimsIdentity)User.Identity;
            var ID = (identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            int userID = int.Parse(ID);
            try
            {
                var shifts = shiftService.UpdateShiftActive(userID, shift);
                result = Ok(shifts);
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

        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        [Route("GetShiftFromLocationByDate")]
        public IActionResult GetShiftFromLocationByDate(int locationId, DateTime date)
        {
            IActionResult result;
            try
            {
                var shifts = shiftService.GetShiftFromLocationByDate(locationId, date);
                result = Ok(shifts);
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

        [Authorize(Roles = Constant.MANAGER)]
        [HttpGet]
        [Route("GetShiftFromLocationByDateManager")]
        public IActionResult GetShiftFromLocationByDateManager(int locationId, DateTime date)
        {
            IActionResult result;
            var identity = (ClaimsIdentity)User.Identity;
            var ID = (identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            int userID = int.Parse(ID);
            try
            {
                var shifts = shiftService.GetShiftFromLocationByDateManager(userID, locationId, date);
                result = Ok(shifts);
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

        [Authorize(Roles = Constant.WORKER)]
        [HttpGet]
        [Route("GetShiftFromLocationByDateWorker")]
        public IActionResult GetShiftFromLocationByDateWorker(int locationId, DateTime date)
        {
            IActionResult result;
            var identity = (ClaimsIdentity)User.Identity;
            var ID = (identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            int userID = int.Parse(ID);
            try
            {
                var shifts = shiftService.GetShiftFromLocationByDateWorker(userID, locationId, date);
                result = Ok(shifts);
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
    }
}