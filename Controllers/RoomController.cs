using DWDW_API.Core.Constants;
using DWDW_API.Core.Infrastructure;
using DWDW_API.Core.ViewModels;
using DWDW_API.Providers;
using DWDW_Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DWDW_API.Controllers
{
    [Route("api/[controller]")]
    public class RoomController : BaseController
    {
        private readonly IRoomService roomService;
        private readonly JwtTokenProvider jwtTokenProvider;
        public RoomController(ExtensionSettings extensionSettings, IRoomService roomService,
            JwtTokenProvider jwtTokenProvider) : base(extensionSettings)
        {
            this.roomService = roomService;
            this.jwtTokenProvider = jwtTokenProvider;
        }

        [HttpGet]
        [Authorize(Roles = Constant.ADMIN)]
        [Route("GetRooms")]
        public dynamic GetRooms()
        {
            return ExecuteInMonitoring(() =>
            {
                return roomService.GetRooms();
            });
        }

        [HttpGet]
        [Authorize(Roles = Constant.ADMIN)]
        [Route("GetRoomsFromLocation/{locationId}")]
        public dynamic GetRoomsFromLocation(int locationId)
        {
            return ExecuteInMonitoring(() =>
            {
                return roomService.GetRoomsFromLocation(locationId);
            });
        }

        [HttpGet]
        [Authorize(Roles = Constant.ADMIN)]
        [Route("GetActiveRoomsFromLocation")]
        public dynamic GetActiveRoomsFromLocation(int locationId)
        {
            return ExecuteInMonitoring(() =>
            {
                return roomService.GetActiveRoomsFromLocation(locationId);
            });
        }

        [HttpGet]
        [Route("GetRoomById")]
        [Authorize(Roles = Constant.ADMIN + "," + Constant.MANAGER)]
        public dynamic GetRoomById(int RoomId)
        {
            return ExecuteInMonitoring(() =>
            {
                return roomService.GetRoomById(RoomId);
            });
        }

        [HttpGet]
        [Route("SearchRoomCodeByAdmin")]
        [Authorize(Roles = Constant.ADMIN)]
        public dynamic SearchRoomCodeByAdmin(string roomCode)
        {
            return ExecuteInMonitoring(() =>
            {
                return roomService.SearchRoomCode(roomCode);
            });
        }

        [HttpPut]
        [Authorize(Roles = Constant.ADMIN)]
        [Route("UpdateRoom")]
        public dynamic UpdateRoom(RoomUpdateModel roomUpdate)
        {
            return ExecuteInMonitoring(() =>
            {
                return roomService.UpdateRoom(roomUpdate);
            });
        }

        [HttpPost]
        [Authorize(Roles = Constant.ADMIN)]
        [Route("CreateRoom")]
        public dynamic CreateRoom(RoomInsertModel roomInsert)
        {
            return ExecuteInMonitoring(() =>
            {
                return roomService.InsertRoom(roomInsert);
            });
        }

        [HttpPut]
        [Authorize(Roles = Constant.ADMIN)]
        [Route("DeactiveRoom/{roomId}")]
        public dynamic DeactiveRoom(int roomId)
        {
            return ExecuteInMonitoring(() =>
            {
                return roomService.DeactiveRoom(roomId);
            });
        }

        [HttpGet]
        [Authorize(Roles = Constant.MANAGER)]
        [Route("GetRoomsFromLocationByManager/{locationId}")]
        public dynamic GetRoomsFromLocationByManager(int locationId)
        {
            return ExecuteInMonitoring(() =>
            {
                int userId = int.Parse(CurrentUserId);
                return roomService.GetRoomsFromLocationByManager(userId, locationId);
            });
        }

        [HttpGet]
        [Route("SearchRoomCodeByManager")]
        [Authorize(Roles = Constant.MANAGER)]
        public dynamic SearchRoomCodeByManager(string roomCode)
        {
            return ExecuteInMonitoring(() =>
            {
                int userId = int.Parse(CurrentUserId);
                return roomService.SearchRoomCodeByManager(userId, roomCode);
            });
        }

        [HttpGet]
        [Authorize(Roles = Constant.ADMIN)]
        [Route("GetUnassignedRoomsFromLocation/{locationId}")]
        public dynamic GetUnassignedRoomsFromLocation(int locationId)
        {
            return ExecuteInMonitoring(() =>
            {
                return roomService.GetUnassignedRoomsFromLocation(locationId);
            });
        }
    }
}