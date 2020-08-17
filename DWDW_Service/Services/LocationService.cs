using DWDW_API.Core.Constants;
using DWDW_API.Core.Entities;
using DWDW_API.Core.Infrastructure;
using DWDW_API.Core.ViewModels;
using DWDW_Service.Repositories;
using DWDW_Service.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DWDW_Service.Services
{
    public interface ILocationService : IBaseService<Location>
    {
        IEnumerable<LocationViewModel> GetLocations();
        IEnumerable<LocationViewModel> GetAllActiveLocations();
        LocationViewModel GetLocationById(int locationId);
        IEnumerable<LocationViewModel> SearchLocationByLocationCode(string locationCode);
        LocationViewModel InsertLocation(LocationInsertModel locationInsert);
        LocationViewModel UpdateLocation(LocationUpdateModel locationUpdate);
        LocationViewModel UpdateLocationStatus(LocationUpdateStatusModel locationStatus);
        LocationViewModel DeactiveLocation(int locationId);
        IEnumerable<LocationViewModel> GetLocationsByManagerWorker(int userId);
        List<LocationRecordViewModel> GetLocationsRecordBetweenDate(DateTime start, DateTime end);
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
                        List<Room> rooms = new List<Room>();
                        if (location.IsActive == true)
                        {
                            //disable arrangements, rooms
                            var arrangements = arrangementRepository.DisableArrangementFromLocation(locationId);
                            rooms = roomRepository.DisableRoomFromLocation(locationId);
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
                        }
                        else
                        {
                            //enable rooms
                            rooms = roomRepository.EnableRoomFromLocation(locationId);
                            location.IsActive = true;
                        }

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

        public LocationViewModel UpdateLocationStatus(LocationUpdateStatusModel locationStatus)
        {
            LocationViewModel result;
            var location = locationRepository.Find(locationStatus.LocationId);
            if (location != null)
            {
                using (var transaction = unitOfWork.CreateTransaction())
                {
                    try
                    {
                        if (locationStatus.IsActive == false)
                        {
                            //disable arrangements, rooms
                            var arrangements = arrangementRepository.DisableArrangementFromLocation(locationStatus.LocationId);
                            var rooms = roomRepository.DisableRoomFromLocation(locationStatus.LocationId);
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
                        }
                        else
                        {
                            location.IsActive = true;
                        }
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

        public IEnumerable<LocationViewModel> GetAllActiveLocations()
        {
            var allLocation = locationRepository.GetAll().Select(l => l.ToViewModel<LocationViewModel>());
            var result = allLocation.Where(x => x.IsActive == true).ToList();
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
                var locationEntity = locationInsert.ToEntity<Location>();
                //DatNDD refactors this code line
                locationEntity.IsActive = true;

                locationRepository.Add(locationEntity);

                //DatNDD refactors this code line

                //result = locationRepository.GetLocationByLocationCode(locationInsert.LocationCode)
                //                           .ToViewModel<LocationViewModel>();

                result = locationEntity.ToViewModel<LocationViewModel>();
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
            if (location == null)
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_EXISTED);
            }
            var checkLocation = locationRepository.CheckLocationCodeExisted(locationUpdate.LocationCode);
            if (checkLocation != null)
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_EXISTED);
            }
            location.LocationCode = locationUpdate.LocationCode;
            location.IsActive = location.IsActive;
            locationRepository.Update(location);
            result = location.ToViewModel<LocationViewModel>();
            return result;
        }

        public IEnumerable<LocationViewModel> GetLocationsByManagerWorker(int userId)
        {
            var arrangementRepository = this.unitOfWork.ArrangementRepository;
            //get Arrangement of Manager 
            var arrangementsOfManager = arrangementRepository.GetArrangementOfUser(userId);
            //get Location which the manager is manage
            var locations = arrangementsOfManager.Select(a => a.Location.ToViewModel<LocationViewModel>());
            return locations;
        }

        public IEnumerable<LocationViewModel> SearchLocationByLocationCode(string locationCode)
        {
            IEnumerable<LocationViewModel> result;
            var locations = locationRepository.SearchByLocationCode(locationCode);
            result = locations.Select(l => l.ToViewModel<LocationViewModel>());
            return result;
        }

        public List<LocationRecordViewModel> GetLocationsRecordBetweenDate(DateTime start, DateTime end)
        {
            List<LocationRecordViewModel> result = new List<LocationRecordViewModel>();
            var locations = locationRepository.GetAll().ToList();
            if (locations == null)
            {
                throw new BaseException(ErrorMessages.GET_LIST_FAIL);
            }
            var recordRepo = this.unitOfWork.RecordRepository;
            float recordNumber = 0;
            foreach (var item in locations)
            {
                if (start == end)
                {
                    recordNumber = (float)recordRepo.GetRecordsByLocationIdAndTime((int)item.LocationId, start).Count();
                    result.Add(new LocationRecordViewModel
                    {
                        LocationId = (int)item.LocationId,
                        LocationCode = item.LocationCode,
                        TotalRecord = recordNumber
                    });
                }
                else
                {
                    if (start.Date > end.Date)
                    {
                        DateTime f;
                        f = start;
                        start = end;
                        end = f;
                    }
                    recordNumber = (float)recordRepo.GetRecordsByLocationIdBetweenTime((int)item.LocationId, start, end).Count();
                    result.Add(new LocationRecordViewModel
                    {
                        LocationId = (int)item.LocationId,
                        LocationCode = item.LocationCode,
                        TotalRecord = recordNumber
                    });
                }
            }
            return result;
        }
    }
}
