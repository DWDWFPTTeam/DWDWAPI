using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DWDW_API.Core.Constants;
using DWDW_API.Core.Entities;
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
        //[Authorize(Roles = Constant.ADMIN)]
        [Route("GetRooms")]
        public IActionResult GetRooms()
        {
            IActionResult result;
            try
            {
                var list = roomService.GetRooms();
                return Ok(list);
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

        [HttpGet]
        //[Authorize(Roles = Constant.ADMIN)]
        [Route("GetRoomsFromLocation/{locationId}")]
        public IActionResult GetRoomsFromLocation(int locationId)
        {
            IActionResult result;
            try
            {
                var list = roomService.GetRoomsFromLocation(locationId);
                return Ok(list);
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

        [HttpGet]
        [Route("GetRoomById")]
        //[Authorize(Roles = Constant.ADMIN)]
        public IActionResult GetRoomById(int RoomId)
        {
            IActionResult result;
            try
            {
                var room = roomService.GetRoomById(RoomId);
                return Ok(room);
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

        [HttpPut]
        //[Authorize(Roles = Constant.ADMIN)]
        [Route("UpdateRoom")]
        public IActionResult UpdateRoom(RoomUpdateModel roomUpdate)
        {
            //if (!ModelState.IsValid) return BadRequest(ModelState);
            IActionResult result;
            try
            {
                var roomUpdated = roomService.UpdateRoom(roomUpdate);
                return Ok(roomUpdated);
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

        [HttpPost]
        //[Authorize(Roles = Constant.ADMIN)]
        [Route("CreateRoom")]
        public IActionResult CreateRoom(RoomInsertModel roomInsert)
        {
            //if (!ModelState.IsValid) return BadRequest(ModelState);
            IActionResult result;
            try
            {
                var roomInserted = roomService.InsertRoom(roomInsert);
                return Ok(roomInserted);
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

        [HttpPut]
        //[Authorize(Roles = Constant.ADMIN)]
        [Route("DeactiveRoom/{roomId}")]
        public IActionResult DeactiveRoom(int roomId)
        {
            //if (!ModelState.IsValid) return BadRequest(ModelState);
            IActionResult result;
            try
            {
                var roomDeactived = roomService.DeactiveRoom(roomId);
                return Ok(roomDeactived);
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