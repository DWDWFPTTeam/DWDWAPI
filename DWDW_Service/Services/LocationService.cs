using DWDW_API.Core.Constants;
using DWDW_API.Core.Entities;
using DWDW_API.Core.Infrastructure;
using DWDW_API.Core.ViewModels;
using DWDW_Service.Repositories;
using DWDW_Service.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DWDW_Service.Services
{
    public interface ILocationService : IBaseService<Location>
    {
        IEnumerable<LocationViewModel> GetLocations();
        LocationViewModel GetLocationById(int locationId);
        LocationViewModel InsertLocation(LocationInsertModel locationInsert);
        LocationViewModel UpdateLocation(LocationUpdateModel locationUpdate);
        LocationViewModel DeactiveLocation(int locationId);
        IEnumerable<Location> GetAssignedLocations(int userId);
    }
    public class LocationService : BaseService<Location>, ILocationService
    {
        private readonly ILocationRepository locationRepository;
        private readonly IArrangementRepository arrangementRepository;
        private readonly IRoomRepository roomRepository;
        private readonly IShiftRepository shiftRepository;
        private readonly IRoomDeviceRepository roomDeviceRepository;

        public LocationService(UnitOfWork unitOfWork, ILocationRepository locationRepository,
            IArrangementRepository arrangementRepository, IRoomRepository roomRepository,
            IShiftRepository shiftRepository, IRoomDeviceRepository roomDeviceRepository)
            : base(unitOfWork)
        {
            this.locationRepository = locationRepository;
            this.arrangementRepository = arrangementRepository;
            this.roomRepository = roomRepository;
            this.shiftRepository = shiftRepository;
            this.roomDeviceRepository = roomDeviceRepository;
        }

        public LocationViewModel DeactiveLocation(int locationId)
        {
            LocationViewModel result;
            //validate
            var location = locationRepository.Find(locationId);
            if (location != null)
            {
                using (var transaction = unitOfWork.CreateTransaction())
                {
                    try
                    {
                        //disable arrangements, rooms
                        var arrangements = arrangementRepository.DisableArrangementFromLocation(locationId);
                        var rooms = roomRepository.DisableRoomFromLocation(locationId);

                        foreach (var a in arrangements)
                        {
                            //disable shifts
                            shiftRepository.DisableShiftsByArrangementId(a.ArrangementId);
                        }
                        foreach (var r in rooms)
                        {
                            //disable roomDevice
                            roomDeviceRepository.DisableRoomDevice(r.RoomId);
                        }
                        location.IsActive = false;
                        locationRepository.Update(location);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
                result = location.ToViewModel<LocationViewModel>();
            }
            else
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_EXISTED);
            }
            return result;
        }

        public IEnumerable<Location> GetAssignedLocations(int userId)
        {
            var result = locationRepository.GetAll()
                    .Where(a => a.Arrangement.Any(u => u.UserId == userId))
                    .ToList();
            if (result == null)
            {
                throw new BaseException(ErrorMessages.GET_LIST_FAIL);
            }
            return result;
        }

        public LocationViewModel GetLocationById(int locationId)
        {
            var location = locationRepository.Find(locationId);
            LocationViewModel result;
            if (location != null)
            {
                result = location.ToViewModel<LocationViewModel>();
            }
            else
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_EXISTED);
            }
            return result;

        }

        public IEnumerable<LocationViewModel> GetLocations()
        {
            var result = locationRepository.GetAll().Select(l => l.ToViewModel<LocationViewModel>());
            if (result == null)
            {
                throw new BaseException(ErrorMessages.GET_LIST_FAIL);
            }
            return result;
        }

        public LocationViewModel InsertLocation(LocationInsertModel locationInsert)
        {
            LocationViewModel result;
            var check = locationRepository.GetLocationByLocationCode(locationInsert.LocationCode);
            if (check == null)
            {
                locationRepository.Add(locationInsert.ToEntity<Location>());
                result = locationRepository.GetLocationByLocationCode(locationInsert.LocationCode)
                                           .ToViewModel<LocationViewModel>();
            }
            else
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_EXISTED);
            }
          
            return result;
        }


        public LocationViewModel UpdateLocation(LocationUpdateModel locationUpdate)
        {
            LocationViewModel result;
            var location = locationRepository.Find(locationUpdate.LocationId);
            if (location != null)
            {
                location.LocationCode = locationUpdate.LocationCode;
                location.IsActive = true;
                locationRepository.Update(location);
                result = location.ToViewModel<LocationViewModel>();
            }
            else
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_EXISTED);
            }
            return result;
        }
    }
}
