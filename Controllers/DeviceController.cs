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
        public dynamic GetAllDevices()
        {
            return ExecuteInMonitoring(() =>
            {
                return deviceService.GetAll();
            });
        }

        //Search device by name
        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        [Route("GetDeviceCode")]
        public dynamic SearchDeviceCode(string deviceCode)
        {
            return ExecuteInMonitoring(() =>
            {
                return deviceService.GetDeviceCode(deviceCode);
            });
        }

        //Search device by ID
        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        [Route("GetDeviceID")]
        public dynamic SearchDeviceID(int deviceID)
        {
            return ExecuteInMonitoring(() =>
            {
                return deviceService.GetDeviceID(deviceID);
            });
        }

        [Authorize(Roles = Constant.ADMIN)]
        [HttpPost]
        [Route("CreateDevice")]
        public dynamic CreateDevice(DeviceCreateModel device)
        {
            return ExecuteInMonitoring(() =>
            {
                return deviceService.CreateDevice(device);
            });    
        }

        [Authorize(Roles = Constant.ADMIN)]
        [HttpPut]
        [Route("UpdateDevice")]
        public dynamic UpdateDevice(DeviceUpdateModel device)
        {
            return ExecuteInMonitoring(() =>
            {
                return deviceService.UpdateDevice(device);
            });
        }

        [Authorize(Roles = Constant.ADMIN)]
        [HttpPut]
        [Route("UpdateDeviceActive")]
        public dynamic UpdateDeviceActive(DeviceActiveModel device)
        {
            return ExecuteInMonitoring(() =>
            {
                return deviceService.UpdateDeviceActive(device);
            });
        }

        //Get device with active relationship with room in a location for admin
        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        [Route("GetActiveDeviceFromLocationAdmin")]
        public dynamic GetDeviceFromLocation(int locationID)
        {
            return ExecuteInMonitoring(() =>
            {
                return deviceService.GetActiveDeviceFromLocation(locationID);
            });
        }

        //Get device belong to manager location
        [Authorize(Roles = Constant.MANAGER)]
        [HttpGet]
        [Route("GetActiveDeviceFromLocationManager")]
        public dynamic GetDeviceFromLocationManager(int locationID)
        {
            int userID = int.Parse(CurrentUserId);
            return ExecuteInMonitoring(() =>
            {
                return deviceService.GetActiveDeviceFromLocationManager(userID, locationID);
            });
        }


        //Get device currently belong to a room
        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        [Route("GetActiveDeviceFromRoomAdmin")]
        public dynamic GetDeviceFromRoom(int roomID)
        {
            return ExecuteInMonitoring(() =>
            {
                return deviceService.GetActiveDeviceFromRoom(roomID);
            });
        }
        //Get device belong to manager room
        [Authorize(Roles = Constant.MANAGER)]
        [HttpGet]
        [Route("GetActiveDeviceFromRoomManager")]
        public dynamic GetDeviceFromRoomManager(int roomID)
        {
            
            int userID = int.Parse(CurrentUserId);
            return ExecuteInMonitoring(() =>
            {
                return deviceService.GetActiveDeviceFromRoomManager(userID, roomID);
            });
        }

        [Authorize(Roles = Constant.ADMIN)]
        [HttpPost]
        [Route("AssignDeviceToRoom")]
        public dynamic AssignDeviceToRoom(RoomDeviceCreateModel roomDevice)
        {
            return ExecuteInMonitoring(() =>
            {
                return deviceService.AssignDeviceToRoom(roomDevice);
            });
        }
    }
}