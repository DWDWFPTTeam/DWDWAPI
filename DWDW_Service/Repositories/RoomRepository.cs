﻿using DWDW_API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DWDW_Service.Repositories
{
    public interface IRoomRepository: IBaseRepository<Room>
    {
        Room GetRoomByRoomCode(string roomCode);
        void DisableDeviceRoom(int? deviceID);
        void DisableRoomDevice(int? roomID);
        RoomDevice GetLatest();
    }
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        public RoomRepository(DbContext dbContext) : base(dbContext)
        {
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
    }
}
