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
        public dynamic GetLocations()
        {
            return ExecuteInMonitoring(() =>
            {
                return locationService.GetLocations();
            });
        }

        [HttpGet]
        [Authorize(Roles = Constant.ADMIN)]
        [Route("GetAllActiveLocations")]
        public dynamic GetAllActiveLocations()
        {
            return ExecuteInMonitoring(() =>
            {
                return locationService.GetAllActiveLocations();
            });
        }

        [HttpGet]
        [Authorize(Roles = Constant.ADMIN)]
        [Route("GetLocationById")]
        public dynamic GetLocationById(int locationId)
        {
            return ExecuteInMonitoring(() =>
            {
                return locationService.GetLocationById(locationId);
            });
        }

        [HttpGet]
        [Authorize(Roles = Constant.ADMIN)]
        [Route("SearchLocationCode")]
        public dynamic SearchLocationByLocationCode(string locationCode)
        {
            return ExecuteInMonitoring(() =>
            {
                return locationService.SearchLocationByLocationCode(locationCode);
            });
        }

        [HttpPut]
        [Authorize(Roles = Constant.ADMIN)]
        [Route("UpdateLocation")]
        public dynamic UpdateLocation(LocationUpdateModel locationUpdate)
        {

            //if (!ModelState.IsValid) return BadRequest(ModelState);
            return ExecuteInMonitoring(() =>
            {
                return locationService.UpdateLocation(locationUpdate);
            });
        }

        [HttpPost]
        [Authorize(Roles = Constant.ADMIN)]
        [Route("CreateLocation")]
        public dynamic CreateLocation(LocationInsertModel locationInsert)
        {
            //if (!ModelState.IsValid) return BadRequest(ModelState);
            return ExecuteInMonitoring(() =>
            {
                return locationService.InsertLocation(locationInsert);
            });
        }

        [HttpPut]
        [Authorize(Roles = Constant.ADMIN)]
        [Route("DeactiveLocation/{locationId}")]
        public dynamic DeactiveLocation(int locationId)
        {
            //if (!ModelState.IsValid) return BadRequest(ModelState);
            return ExecuteInMonitoring(() =>
            {
                return locationService.DeactiveLocation(locationId);
            });
        }

        [HttpPut]
        [Authorize(Roles = Constant.ADMIN)]
        [Route("UpdateLocationStatus")]
        public dynamic UpdateLocationStatus(LocationUpdateStatusModel location)
        {
            //if (!ModelState.IsValid) return BadRequest(ModelState);
            return ExecuteInMonitoring(() =>
            {
                return locationService.UpdateLocationStatus(location);
            });
        }

        [HttpGet]
        [Authorize(Roles = Constant.MANAGER + ", " + Constant.WORKER)]
        [Route("GetLocationsByManagerWorker")]
        public dynamic GetLocationsByManagerWorker()
        {
            int userId = int.Parse(CurrentUserId);
            return ExecuteInMonitoring(() =>
            {
                return locationService.GetLocationsByManagerWorker(userId);
            });
        }

        [HttpGet]
        [AllowAnonymous]
        [Authorize(Roles = Constant.ADMIN)]
        [Route("GetLocationsRecord")]
        public dynamic GetLocationsRecord([FromQuery] LocationReceiveDateModel receiveDateModel)
        {
            DateTime end_lastDate = receiveDateModel.endDate.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
            //check date is valid
            List<LocationRecordViewModel> list = new List<LocationRecordViewModel>();
            return ExecuteInMonitoring(() =>
            {
                return locationService.GetLocationsRecordBetweenDate(receiveDateModel.startDate, end_lastDate);
            });
        }
    }
}