using DWDW_API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DWDW_Service.Repositories
{
    public interface IRoomRepository: IBaseRepository<Room>
    {
        List<Room> GetRoomFromLocation(int locationID);
        bool CheckRoomLocation(int? roomID, int? ArrangementID);
    }
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        public RoomRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public List<Room> GetRoomFromLocation(int locationID)
        {
            return this.dbContext.Set<Room>().Where(x => x.LocationId == locationID).ToList();
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
    }
}
