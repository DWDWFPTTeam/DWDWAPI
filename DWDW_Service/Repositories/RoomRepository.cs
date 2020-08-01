using DWDW_API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DWDW_Service.Repositories
{
    public interface IRoomRepository : IBaseRepository<Room>
    {
        Room GetRoomByRoomCode(string roomCode);
        void DisableDeviceRoom(int? deviceID);
        void DisableRoomDevice(int? roomID);
        RoomDevice GetLatest();
        List<Room> GetRoomFromLocation(int locationID);
        List<Room> DisableRoomFromLocation(int locationID);
        List<Room> EnableRoomFromLocation(int locationID);
        bool CheckRoomLocation(int? roomID, int? ArrangementID);
        List<Room> SearchRoomByRoomCode(string roomCode);
        List<Room> SearchRoomByRoomCode(int locationId, string roomCode);

        List<int?> GetRelatedRoomIDFromLocation(List<int?> locationRelatedID);
    }
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        public RoomRepository(DbContext dbContext) : base(dbContext)
        {
        }


        public List<Room> GetRoomFromLocation(int locationID)
        {
            return this.dbContext.Set<Room>()
                .Where(x => x.LocationId == locationID)
                .ToList();
        }
        public Room GetRoomByRoomCode(string roomCode)
        {
            return this.dbContext.Set<Room>().FirstOrDefault
                 (r => r.RoomCode.Trim().ToLower().Equals(roomCode.Trim().ToLower()));
        }
        public void DisableDeviceRoom(int? deviceID)
        {
            var deviceRoom = this.dbContext.Set<RoomDevice>().Where(x => x.DeviceId == deviceID).ToList();
            deviceRoom.ForEach(a => a.IsActive = false);
            this.dbContext.SaveChanges();
        }
        public void DisableRoomDevice(int? roomID)
        {
            var roomDevice = this.dbContext.Set<RoomDevice>().Where(x => x.RoomId == roomID).ToList();
            roomDevice.ForEach(a => a.IsActive = false);
            this.dbContext.SaveChanges();
        }

        public RoomDevice GetLatest()
        {
            return this.dbContext.Set<RoomDevice>().OrderByDescending(x => x.RoomDeviceId).First();
        }
        public bool CheckRoomLocation(int? roomID, int? ArrangementID)
        {
            bool result = false;
            var arrangement = this.dbContext.Set<Arrangement>().Find(ArrangementID);
            var roomLocation = this.dbContext.Set<Room>().Find(roomID);
            if (roomLocation != null && arrangement != null)
            {
                if (roomLocation.LocationId == arrangement.LocationId)
                {
                    result = true;
                }
            }
            return result;
        }

        public List<Room> DisableRoomFromLocation(int locationID)
        {
            var rooms = this.dbContext.Set<Room>().Where(x => x.LocationId == locationID).ToList();
            rooms.ForEach(r => r.IsActive = false);
            this.dbContext.SaveChanges();
            return rooms;
        }

        public List<Room> SearchRoomByRoomCode(string roomCode)
        {
            return this.dbContext.Set<Room>()
                    .Where(r => r.RoomCode.Contains(roomCode))
                    .ToList();
        }

        public List<Room> SearchRoomByRoomCode(int locationId, string roomCode)
        {
            return this.dbContext.Set<Room>()
                    .Where(r => r.LocationId == locationId)
                    .Where(r => r.RoomCode.Contains(roomCode))
                    .Where(r => r.IsActive == true)
                    .ToList();
        }

        public List<Room> EnableRoomFromLocation(int locationID)
        {
            var rooms = this.dbContext.Set<Room>().Where(x => x.LocationId == locationID).ToList();
            rooms.ForEach(r => r.IsActive = true);
            this.dbContext.SaveChanges();
            return rooms;
        }

        public List<int?> GetRelatedRoomIDFromLocation(List<int?> locationRelatedID)
        {
            List<int?> relatedRoom = new List<int?>();
            var roomRelated = dbContext.Set<Room>().Where(x => locationRelatedID.Contains(x.LocationId) && x.IsActive == true).ToList();
            for (int i = 0; i < roomRelated.Count; i++)
            {
                int? roomID = roomRelated.ElementAt(i).RoomId;
                relatedRoom.Add(roomID);
            }
            return relatedRoom;
        }
    }
}
