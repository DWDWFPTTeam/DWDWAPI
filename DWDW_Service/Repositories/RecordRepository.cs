using DWDW_API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DWDW_Service.Repositories
{
    public interface IRecordRepository : IBaseRepository<Record>
    {
        User GetDeviceToken(int deviceID);
        string GetRoom(int deviceID);
        List<Record> GetRecordsByLocationId(int locationId);
        List<Record> GetRecordsByLocationIdBetweenTime
            (int locationId, DateTime startDate, DateTime endDate);
    }
    public class RecordRepository : BaseRepository<Record>, IRecordRepository
    {
        public RecordRepository(DbContext dbContext) : base(dbContext)
        {

        }

        public User GetDeviceToken(int deviceID)
        {
            var roomDevice = dbContext.Set<RoomDevice>().FirstOrDefault(x => x.DeviceId == deviceID && x.IsActive == true);
            var room = dbContext.Set<Room>().FirstOrDefault(x => x.RoomId == roomDevice.RoomId);
            var location = dbContext.Set<Location>().FirstOrDefault(x => x.LocationId == room.LocationId);
            var userRelated = dbContext.Set<Arrangement>().FirstOrDefault(a => a.LocationId == location.LocationId && a.User.RoleId == 2 && a.IsActive == true);        
            var manager = dbContext.Set<User>().Find(userRelated.UserId);
            return manager;
        }

        public List<Record> GetRecordsByLocationId(int locationId)
        {
            var result = from record in dbContext.Set<Record>()
                         join rd in dbContext.Set<RoomDevice>() 
                            on record.DeviceId equals rd.DeviceId
                         join room in dbContext.Set<Room>() 
                            on rd.RoomId equals room.RoomId
                         where room.IsActive == true 
                            && room.LocationId == locationId
                         orderby record.RecordDateTime ascending
                         select record;
            return result.ToList();
        }

        public List<Record> GetRecordsByLocationIdBetweenTime
            (int locationId, DateTime startDate, DateTime endDate)
        {
            var result = from record in dbContext.Set<Record>()
                         join 
                            rd in dbContext.Set<RoomDevice>()
                            on record.DeviceId equals rd.DeviceId
                         join 
                            room in dbContext.Set<Room>()
                            on rd.RoomId equals room.RoomId
                         where 
                            room.IsActive == true
                            && room.LocationId == locationId
                            && record.RecordDateTime >= startDate
                            && record.RecordDateTime <= endDate
                         orderby record.RecordDateTime ascending
                         select record;
            return result.ToList();
        }

        public string GetRoom(int deviceID)
        {
            var roomDevice = dbContext.Set<RoomDevice>().FirstOrDefault(x => x.DeviceId == deviceID && x.IsActive == true);
            var room = dbContext.Set<Room>().FirstOrDefault(x => x.RoomId == roomDevice.RoomId);
            return room.RoomCode;
        }
    }
}
