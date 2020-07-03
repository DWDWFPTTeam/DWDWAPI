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
    }
}
