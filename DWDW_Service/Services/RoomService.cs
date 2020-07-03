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
    }
    public class RoomService : BaseService<Room>, IRoomService
    {
        private readonly IRoomRepository roomRepository;
        private readonly ILocationRepository locationRepository;
        public RoomService(UnitOfWork unitOfWork, IRoomRepository roomRepository,
            ILocationRepository locationRepository) : base(unitOfWork)
        {
            this.roomRepository = roomRepository;
            this.locationRepository = locationRepository;
        }

        public RoomViewModel DeactiveRoom(int roomId)
        {
            RoomViewModel result;
            //validate
            var room = roomRepository.Find(roomId);
            if (room!=null)
            {
                room.IsActive = false;
                roomRepository.Update(room);
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
            if (room!=null)
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
            var result = roomRepository.GetAll()
                .Where(r => r.LocationId == locationId)
                .Select(r => r.ToViewModel<RoomViewModel>());
            if (result==null)
            {
                throw new BaseException(ErrorMessages.GET_LIST_FAIL);
            }
            return result;
        }

        public RoomViewModel InsertRoom(RoomInsertModel roomInsert)
        {
            RoomViewModel result = null;
            Room room; 

            var checkRoom = roomRepository.GetRoomByRoomCode(roomInsert.RoomCode);
            var checkLocation = locationRepository.Find(roomInsert.LocationId);
            
            if (checkRoom == null && checkLocation != null )
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
