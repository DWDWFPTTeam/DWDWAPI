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
    public class DeviceController : BaseController
    {
        private readonly IDeviceService deviceService;
        public DeviceController(ExtensionSettings extensionSettings, IDeviceService deviceService) : base(extensionSettings)
        {
            this.deviceService = deviceService;
        }

        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        [Route("GetAllDevice")]
        public IActionResult GetAllDevices()
        {
            IActionResult result;
            try
            {
                var devices = deviceService.GetAll();
                result = Ok(devices);
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

        //Search device by name
        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        [Route("GetDeviceCode")]
        public IActionResult SearchDeviceCode(string deviceCode)
        {
            IActionResult result;
            try
            {
                var devices = deviceService.GetDeviceCode(deviceCode);
                result = Ok(devices);
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

        //Search device by ID
        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        [Route("GetDeviceID")]
        public IActionResult SearchDeviceID(int deviceID)
        {
            IActionResult result;
            try
            {
                var devices = deviceService.GetDeviceID(deviceID);
                result = Ok(devices);
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
        [HttpPost]
        [Route("CreateDevice")]
        public IActionResult CreateDevice(DeviceCreateModel device)
        {
            IActionResult result;
            try
            {
                var devices = deviceService.CreateDevice(device);
                result = Ok(devices);
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
        [HttpPut]
        [Route("UpdateDevice")]
        public IActionResult UpdateDevice(DeviceUpdateModel device)
        {
            IActionResult result;
            try
            {
                var devices = deviceService.UpdateDevice(device);
                result = Ok(devices);
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
        [HttpPut]
        [Route("UpdateDeviceActive")]
        public IActionResult UpdateDeviceActive(DeviceActiveModel device)
        {
            IActionResult result;
            try
            {
                var devices = deviceService.UpdateDeviceActive(device);
                result = Ok(devices);
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

        //Get device with active relationship with room in a location for admin
        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        [Route("GetActiveDeviceFromLocationAdmin")]
        public IActionResult GetDeviceFromLocation(int locationID)
        {
            IActionResult result;
            try
            {
                var devices = deviceService.GetActiveDeviceFromLocation(locationID);
                result = Ok(devices);
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

        //Get device belong to manager location
        [Authorize(Roles = Constant.MANAGER)]
        [HttpGet]
        [Route("GetActiveDeviceFromLocationManager")]
        public IActionResult GetDeviceFromLocationManager(int locationID)
        {
            IActionResult result;
            var identity = (ClaimsIdentity)User.Identity;
            var ID = (identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            int userID = int.Parse(ID);
            try
            {
                var devices = deviceService.GetActiveDeviceFromLocationManager(userID,locationID);
                result = Ok(devices);
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


        //Get device currently belong to a room
        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        [Route("GetActiveDeviceFromRoomAdmin")]
        public IActionResult GetDeviceFromRoom(int roomID)
        {
            IActionResult result;
            try
            {
                var devices = deviceService.GetActiveDeviceFromRoom(roomID);
                result = Ok(devices);
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
        //Get device belong to manager room
        [Authorize(Roles = Constant.MANAGER)]
        [HttpGet]
        [Route("GetActiveDeviceFromRoomManager")]
        public IActionResult GetDeviceFromRoomManager(int roomID)
        {
            IActionResult result;
            var identity = (ClaimsIdentity)User.Identity;
            var ID = (identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            int userID = int.Parse(ID);
            try
            {
                var devices = deviceService.GetActiveDeviceFromRoomManager(userID, roomID);
                result = Ok(devices);
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
        [HttpPost]
        [Route("AssignDeviceToRoom")]
        public IActionResult AssignDeviceToRoom(RoomDeviceCreateModel roomDevice)
        {
            IActionResult result;
            try
            {
                var devices = deviceService.AssignDeviceToRoom(roomDevice);
                result = Ok(devices);
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