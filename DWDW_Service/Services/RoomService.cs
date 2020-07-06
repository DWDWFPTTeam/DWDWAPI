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
    public interface IRoomService : IBaseService<Room>
    {
        IEnumerable<RoomViewModel> GetRooms();
        RoomViewModel GetRoomById(int roomId);
        RoomViewModel InsertRoom(RoomInsertModel roomInsert);
        RoomViewModel UpdateRoom(RoomUpdateModel roomUpdate);
        RoomViewModel DeactiveRoom(int roomId);
        IEnumerable<RoomViewModel> GetRoomsFromLocation(int locationId);
        IEnumerable<RoomViewModel> SearchRoomCode(string roomCode);
        IEnumerable<RoomViewModel> GetRoomsFromLocationByManager(int userId, int locationId);
        IEnumerable<RoomViewModel> SearchRoomCodeByManager(int userId, string roomCode);
    }
    public class RoomService : BaseService<Room>, IRoomService
    {
        private readonly IRoomRepository roomRepository;
        private readonly ILocationRepository locationRepository;
        private readonly IRoomDeviceRepository roomDeviceRepository;
        public RoomService(UnitOfWork unitOfWork, IRoomRepository roomRepository,
            ILocationRepository locationRepository, IRoomDeviceRepository roomDeviceRepository)
            : base(unitOfWork)
        {
            this.roomRepository = roomRepository;
            this.locationRepository = locationRepository;
            this.roomDeviceRepository = roomDeviceRepository;
        }

        public RoomViewModel DeactiveRoom(int roomId)
        {
            RoomViewModel result;
            //validate
            var room = roomRepository.Find(roomId);
            if (room != null)
            {
                using (var transaction = unitOfWork.CreateTransaction())
                {
                    try
                    {
                        room.IsActive = false;
                        roomDeviceRepository.DisableRoomDevice(roomId);
                        roomRepository.Update(room);
                        transaction.Commit();
                    }
                    catch (BaseException)
                    {
                        transaction.Rollback();
                        throw new BaseException(ErrorMessages.DEACTIVE_ERROR);
                    }
                }

                result = room.ToViewModel<RoomViewModel>();
            }
            else
            {
                throw new BaseException(ErrorMessages.ROOM_IS_NOT_EXISTED);
            }
            return result;
        }

        public RoomViewModel GetRoomById(int roomId)
        {
            RoomViewModel result = null;
            var room = roomRepository.Find(roomId);
            if (room != null)
            {
                result = room.ToViewModel<RoomViewModel>();
            }
            else
            {
                throw new BaseException(ErrorMessages.ROOM_IS_NOT_EXISTED);
            }
            return result;
        }

        public IEnumerable<RoomViewModel> GetRooms()
        {
            var result = roomRepository.GetAll().Select(r => r.ToViewModel<RoomViewModel>());
            if (result == null)
            {
                throw new BaseException(ErrorMessages.GET_LIST_FAIL);
            }
            return result;
        }

        public IEnumerable<RoomViewModel> GetRoomsFromLocation(int locationId)
        {
            var result = roomRepository.GetRoomFromLocation(locationId)
                .Select(r => r.ToViewModel<RoomViewModel>());
            if (result == null)
            {
                throw new BaseException(ErrorMessages.GET_LIST_FAIL);
            }
            return result;
        }

        public IEnumerable<RoomViewModel> GetRoomsFromLocationByManager(int userId, int locationId)
        {
            IEnumerable<RoomViewModel> result = null;
            if (userId != 0 && locationId != 0)
            {
                var arrangementRepository = this.unitOfWork.ArrangementRepository;
                //get Arrangement of Manager 
                var arrangementsOfManager = arrangementRepository.GetArrangementOfUser(userId);
                //get Location which the manager is manage
                var location = arrangementsOfManager
                                .Where(a => a.LocationId == locationId)
                                .Select(a => a.Location.LocationId)
                                .FirstOrDefault();
                if (location != null)
                {
                    result = roomRepository
                        .GetRoomFromLocation((int)location)
                        .Select(r => r.ToViewModel<RoomViewModel>());
                }
                else
                {
                    throw new BaseException(ErrorMessages.LOCATION_USER_NOT_EXISTED);
                }
            }
            else
            {
                throw new BaseException(ErrorMessages.INVALID_USERID_OR_LOCATIONID);
            }
            return result;
        }

        public RoomViewModel InsertRoom(RoomInsertModel roomInsert)
        {
            RoomViewModel result = null;
            Room room;

            var checkRoom = roomRepository.GetRoomByRoomCode(roomInsert.RoomCode);
            var checkLocation = locationRepository.Find(roomInsert.LocationId);

            if (checkRoom == null && checkLocation != null)
            {
                room = new Room()
                {
                    RoomCode = roomInsert.RoomCode,
                    LocationId = roomInsert.LocationId,
                    IsActive = true
                };
                roomRepository.Add(room);
                result = room.ToViewModel<RoomViewModel>();
            }
            else if (checkRoom != null)
            {
                throw new BaseException(ErrorMessages.ROOM_IS_EXISTED);
            }
            else if (checkLocation == null)
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_EXISTED);
            }
            else
            {
                throw new BaseException(ErrorMessages.INSERT_ERROR);
            }
            return result;
        }

        public IEnumerable<RoomViewModel> SearchRoomCode(string roomCode)
        {
            IEnumerable<RoomViewModel> result;
            var rooms = roomRepository.SearchRoomByRoomCode(roomCode);
            result = rooms.Select(r => r.ToViewModel<RoomViewModel>());
            return result;
        }

        public IEnumerable<RoomViewModel> SearchRoomCodeByManager(int userId, string roomCode)
        {
            List<RoomViewModel> list = new List<RoomViewModel>();
            var arrangementRepository = this.unitOfWork.ArrangementRepository;
            //get Arrangement of Manager 
            var arrangementsOfManager = arrangementRepository.GetArrangementOfUser(userId);
            //get Location which the manager is manage
            var listLocationId = arrangementsOfManager.Select(a => a.Location.LocationId);
            foreach (var locationId in listLocationId)
            {
                var rooms = roomRepository
                    .SearchRoomByRoomCode((int)locationId, roomCode)
                    .Select(r => r.ToViewModel<RoomViewModel>());
                foreach (var room in rooms)
                {
                    list.Add(room);
                }
            }
            return list;
        }

        public RoomViewModel UpdateRoom(RoomUpdateModel roomUpdate)
        {
            RoomViewModel result;
            var room = roomRepository.Find(roomUpdate.RoomId);
            var location = locationRepository.Find(roomUpdate.LocationId);
            if ((room != null) && (location != null))
            {
                room.RoomCode = roomUpdate.RoomCode;
                room.LocationId = room.LocationId;
                room.IsActive = true;
                roomRepository.Update(room);
                result = room.ToViewModel<RoomViewModel>();
            }
            else if (room == null)
            {
                throw new BaseException(ErrorMessages.ROOM_IS_NOT_EXISTED);
            }
            else
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_EXISTED);
            }
            return result;
        }
    }
}
