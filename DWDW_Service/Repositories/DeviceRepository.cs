using DWDW_API.Core.Entities;
using DWDW_API.Core.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DWDW_Service.Repositories
{
    public interface IDeviceRepository : IBaseRepository<Device>
    {
        List<Device> GetDeviceByCode(string deviceCode);
        Device GetDeviceCode(string deviceCode);
        Device CheckDeviceCodeExisted(string deviceCode);
        Device GetDeviceFromRoom(int? roomID);
        bool CheckUserLocation(int userID, int locationID);
        bool CheckUserRoom(int userID, int roomID);

        string GetRoomCode(int? deviceID);
        int? GetRoomID(int? deviceID);
        int? GetLocationID(int? deviceID);
        string GetLocationCode(int? deviceID);
        DateTime? GetStartDate(int? deviceID);
        DateTime? GetEndDate(int? deviceID);
    }
    public class DeviceRepository : BaseRepository<Device>, IDeviceRepository
    {
        public DeviceRepository(DbContext dbContext) : base(dbContext)
        {

        }

        

        public List<Device> GetDeviceByCode(string deviceCode)
        {
            return this.dbContext.Set<Device>().Where(x => x.DeviceCode.Contains(deviceCode)).ToList();
        }
        public Device GetDeviceCode(string deviceCode)
        {
            return this.dbContext.Set<Device>().FirstOrDefault(x => x.DeviceCode == deviceCode);
        }
        public Device CheckDeviceCodeExisted(string deviceCode)
        {
            return this.dbContext.Set<Device>().FirstOrDefault(c => c.DeviceCode == deviceCode && c.IsActive == true);
        }

        public Device GetDeviceFromRoom(int? roomID)
        {
            return this.dbContext.Set<Device>().FirstOrDefault(x => x.RoomDevice.Any(b => b.RoomId == roomID
            && b.IsActive == true));
        }

        public bool CheckUserLocation(int userID, int locationID)
        {
            bool result = false;
            var UserLocation = this.dbContext.Set<Arrangement>().FirstOrDefault(x => x.UserId == userID
            && x.LocationId == locationID);
            if(UserLocation != null)
            {
                result = true;
            }
            return result;
        }

        public bool CheckUserRoom(int userID, int roomID)
        {
            bool result = false;
            var room = this.dbContext.Set<Room>().Find(roomID);
            var location = this.dbContext.Set<Location>().FirstOrDefault(x => x.LocationId == room.LocationId);
            var UserLocation = this.dbContext.Set<Arrangement>().FirstOrDefault(x => x.UserId == userID
            && x.LocationId == location.LocationId);
            if (UserLocation != null)
            {
                result = true;
            }
            return result;
        }


        public string GetRoomCode(int? deviceID)
        {
            string roomCode;
            var roomDevice = dbContext.Set<RoomDevice>().FirstOrDefault(x => x.DeviceId == deviceID && x.IsActive == true);
            if (roomDevice != null)
            {
                var room = dbContext.Set<Room>().Find(roomDevice.RoomId);
                roomCode = room.RoomCode;
            }
            else
            {
                roomCode = "";
            }
            return roomCode;
        }
        public int? GetRoomID(int? deviceID)
        {
            int? roomId;
            var roomDevice = dbContext.Set<RoomDevice>().FirstOrDefault(x => x.DeviceId == deviceID && x.IsActive == true);
            if (roomDevice != null)
            {
                var room = dbContext.Set<Room>().Find(roomDevice.RoomId);
                roomId = room.RoomId;
            }
            else
            {
                roomId = null;
            }
            return roomId;
        }
        public int? GetLocationID(int? deviceID)
        {
            int? locationID;
            var roomDevice = dbContext.Set<RoomDevice>().FirstOrDefault(x => x.DeviceId == deviceID && x.IsActive == true);
            if (roomDevice != null)
            {
                var room = dbContext.Set<Room>().Find(roomDevice.RoomId);
                var location = dbContext.Set<Location>().Find(room.LocationId);
                locationID = location.LocationId;
            }
            else
            {
                locationID = null;
            }
            return locationID;
        }
        public string GetLocationCode(int? deviceID)
        {
            string locationCode;
            var roomDevice = dbContext.Set<RoomDevice>().FirstOrDefault(x => x.DeviceId == deviceID && x.IsActive == true);
            if (roomDevice != null)
            {
                var room = dbContext.Set<Room>().Find(roomDevice.RoomId);
                var location = dbContext.Set<Location>().Find(room.LocationId);
                locationCode = location.LocationCode;
            }
            else
            {
                locationCode = "";
            }
            return locationCode;
        }
        public DateTime? GetStartDate(int? deviceID)
        {
            DateTime? startDate;
            var roomDevice = dbContext.Set<RoomDevice>().FirstOrDefault(x => x.DeviceId == deviceID && x.IsActive == true);
            if (roomDevice != null)
            {
                startDate = roomDevice.StartDate;
            }
            else
            {
                startDate = null;
            }
            return startDate;
        }
        public DateTime? GetEndDate(int? deviceID)
        {
            DateTime? endDate;
            var roomDevice = dbContext.Set<RoomDevice>().FirstOrDefault(x => x.DeviceId == deviceID && x.IsActive == true);
            if (roomDevice != null)
            {
                endDate = roomDevice.EndDate;
            }
            else
            {
                endDate = null;
            }
            return endDate;
        }
    }
}
