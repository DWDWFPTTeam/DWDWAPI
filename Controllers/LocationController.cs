using DWDW_API.Core.Constants;
using DWDW_API.Core.Infrastructure;
using DWDW_API.Core.ViewModels;
using DWDW_API.Providers;
using DWDW_Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace DWDW_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : BaseController
    {
        private readonly ILocationService locationService;
        private readonly JwtTokenProvider jwtTokenProvider;
        public LocationController(ExtensionSettings extensionSettings, ILocationService locationService,
            JwtTokenProvider jwtTokenProvider) : base(extensionSettings)
        {
            this.locationService = locationService;
            this.jwtTokenProvider = jwtTokenProvider;
        }

        [HttpGet]
        [Authorize(Roles = Constant.ADMIN)]
        [Route("GetLocations")]
        public IActionResult GetLocations()
        {
            IActionResult result;
            try
            {
                var list = locationService.GetLocations();
                return Ok(list);
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

        [HttpGet]
        [Authorize(Roles = Constant.ADMIN)]
        [Route("GetAllActiveLocations")]
        public IActionResult GetAllActiveLocations()
        {
            IActionResult result;
            try
            {
                var list = locationService.GetAllActiveLocations();
                return Ok(list);
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

        [HttpGet]
        [Authorize(Roles = Constant.ADMIN)]
        [Route("GetLocationById")]
        public IActionResult GetLocationById(int locationId)
        {
            IActionResult result;
            try
            {
                var location = locationService.GetLocationById(locationId);
                return Ok(location);
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

        [HttpGet]
        [Authorize(Roles = Constant.ADMIN)]
        [Route("SearchLocationCode")]
        public IActionResult SearchLocationByLocationCode(string locationCode)
        {
            IActionResult result;
            try
            {
                var location = locationService.SearchLocationByLocationCode(locationCode);
                return Ok(location);
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

        [HttpPut]
        [Authorize(Roles = Constant.ADMIN)]
        [Route("UpdateLocation")]
        public IActionResult UpdateLocation(LocationUpdateModel locationUpdate)
        {
            //if (!ModelState.IsValid) return BadRequest(ModelState);
            IActionResult result;
            try
            {
                var locationUpdated = locationService.UpdateLocation(locationUpdate);
                return Ok(locationUpdated);
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

        [HttpPost]
        [Authorize(Roles = Constant.ADMIN)]
        [Route("CreateLocation")]
        public IActionResult CreateLocation(LocationInsertModel locationInsert)
        {
            //if (!ModelState.IsValid) return BadRequest(ModelState);
            IActionResult result;
            try
            {
                var locationInserted = locationService.InsertLocation(locationInsert);
                return Ok(locationInserted);
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

        [HttpPut]
        [Authorize(Roles = Constant.ADMIN)]
        [Route("DeactiveLocation/{locationId}")]
        public IActionResult DeactiveLocation(int locationId)
        {
            //if (!ModelState.IsValid) return BadRequest(ModelState);
            IActionResult result;
            try
            {
                var locationDeactived = locationService.DeactiveLocation(locationId);
                return Ok(locationDeactived);
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

        [HttpPut]
        [Authorize(Roles = Constant.ADMIN)]
        [Route("UpdateLocationStatus")]
        public IActionResult UpdateLocationStatus(LocationUpdateStatusModel location)
        {
            //if (!ModelState.IsValid) return BadRequest(ModelState);
            IActionResult result;
            try
            {
                var locationDeactived = locationService.UpdateLocationStatus(location);
                return Ok(locationDeactived);
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

        [HttpGet]
        [Authorize(Roles = Constant.MANAGER + ", " + Constant.WORKER)]
        [Route("GetLocationsByManagerWorker")]
        public IActionResult GetLocationsByManagerWorker()
        {
            IActionResult result;
            try
            {
                int userId = int.Parse(CurrentUserId);
                var list = locationService.GetLocationsByManagerWorker(userId);
                return Ok(list);
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

        [HttpGet]
        [AllowAnonymous]
        [Authorize(Roles = Constant.ADMIN)]
        [Route("GetLocationsRecord")]
        public IActionResult GetLocationsRecord([FromQuery] LocationReceiveDateModel receiveDateModel)
        {
            IActionResult result;
            try
            {
                DateTime end_lastDate = receiveDateModel.endDate.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
                //check date is valid
                List<LocationRecordViewModel> list = new List<LocationRecordViewModel>();
                list = locationService.GetLocationsRecordBetweenDate(receiveDateModel.startDate, end_lastDate);
                return Ok(list);
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