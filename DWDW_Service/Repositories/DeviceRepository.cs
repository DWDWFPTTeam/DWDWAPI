﻿using DWDW_API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return this.dbContext.Set<Device>().FirstOrDefault(c => c.DeviceCode == deviceCode);
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
    }
}
