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
        Room GetRoomByRoomCode(string roomCode);
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
    }
}
