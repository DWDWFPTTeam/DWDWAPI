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
                result = BadRequest(e.Message);
            }
            catch (Exception e)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, e.Message);

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
                result = BadRequest(e.Message);
            }
            catch (Exception e)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, e.Message);

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
        public IActionResult CreateShift(ShiftCreateModel shift)
        {
            IActionResult result;
            var identity = (ClaimsIdentity)User.Identity;
            var ID = (identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            int userID = int.Parse(ID);
            try
            {
                var shifts = shiftService.CreateShift(userID ,shift);
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
        [HttpPut]
        [Route("UpdateShift")]
        public IActionResult UpdateShift(ShiftUpdateModel shift)
        {
            IActionResult result;
            var identity = (ClaimsIdentity)User.Identity;
            var ID = (identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            int userID = int.Parse(ID);
            try
            {
                var shifts = shiftService.UpdateShift(userID, shift);
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



    }
}