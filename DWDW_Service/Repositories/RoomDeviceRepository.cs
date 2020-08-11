using DWDW_API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DWDW_Service.Repositories
{
    public interface IRoomDeviceRepository : IBaseRepository<RoomDevice>
    {
        void DisableDeviceRoom(int? deviceID);
        void DisableRoomDevice(int? roomID);
        RoomDevice GetLatest();
        List<int?> GetRelatedDeviceIDFromRoom(List<int?> roomRelatedID);

        List<RoomDevice> GetOverdue();
    }
    public class RoomDeviceRepository : BaseRepository<RoomDevice>, IRoomDeviceRepository
    {
        public RoomDeviceRepository(DbContext dbContext) : base(dbContext)
        {
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

        public List<int?> GetRelatedDeviceIDFromRoom(List<int?> roomRelatedID)
        {
            List<int?> relatedDevice = new List<int?>();
            var deviceRelated = dbContext.Set<RoomDevice>().Where(x => roomRelatedID.Contains(x.RoomId) && x.IsActive == true).ToList();
            for (int i = 0; i < deviceRelated.Count; i++)
            {
                int? deviceID = deviceRelated.ElementAt(i).DeviceId;
                relatedDevice.Add(deviceID);
            }
            return relatedDevice;
        }

        public List<RoomDevice> GetOverdue()
        {
            DateTime now = DateTime.Now;
            return dbContext.Set<RoomDevice>().Where(x => x.EndDate < now && x.IsActive == true).ToList();
        }
    }
}
