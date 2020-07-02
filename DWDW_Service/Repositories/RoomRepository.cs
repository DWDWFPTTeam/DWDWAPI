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
    }
}
