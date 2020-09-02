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
        IEnumerable<Record> GetRecordsByLocationId(int locationId);
        List<Record> GetRecordsByLocationIdBetweenTime
            (int locationId, DateTime startDate, DateTime endDate);
        List<Record> GetRecordsByLocationIdAndTime
            (int locationId, DateTime date);

        int? GetShiftRoomByArrangementDate(List<int?> arrangementRelated, DateTime date);
        List<int?> GetRelatedArrangement(int workerID);
        List<Record> GetRecordByWorkerDate(int? roomID, DateTime date);
        List<Record> GetRecordByDeviceDate(int? deviceID, DateTime date);
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

        public IEnumerable<Record> GetRecordsByLocationId(int locationId)
        {
            var result = from record in dbContext.Set<Record>()
                         join rd in dbContext.Set<RoomDevice>() 
                            on record.DeviceId equals rd.DeviceId
                         join room in dbContext.Set<Room>() 
                            on rd.RoomId equals room.RoomId
                         where room.IsActive == true 
                            && room.LocationId == locationId
                         orderby record.RecordId descending
                         select record;
            return result;
        }

        public List<Record> GetRecordsByLocationIdAndTime(int locationId, DateTime date)
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
                            && record.RecordDateTime == date
                         orderby record.RecordId descending
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
                         orderby record.RecordId descending
                         select record;
            return result.ToList();
        }

        public string GetRoom(int deviceID)
        {
            var roomDevice = dbContext.Set<RoomDevice>().FirstOrDefault(x => x.DeviceId == deviceID && x.IsActive == true);
            var room = dbContext.Set<Room>().FirstOrDefault(x => x.RoomId == roomDevice.RoomId);
            return room.RoomCode;
        }

        public int? GetShiftRoomByArrangementDate(List<int?> arrangementRelated, DateTime date)
        {
            var result = dbContext.Set<Shift>().FirstOrDefault(x => arrangementRelated.Contains(x.ArrangementId)
            && x.Date == date && x.IsActive == true);
            int? workerRoomID;
            if (result == null)
            {
                workerRoomID = null;
            }
            else
            {
                workerRoomID = result.RoomId;
            }
            
            return workerRoomID;
        }
        public List<int?> GetRelatedArrangement(int workerID)
        {
            var arrangementUser = dbContext.Set<Arrangement>().Where(x => x.UserId == workerID && x.IsActive == true).ToList();
            List<int?> relatedArrangement = new List<int?>();
            for (int i = 0; i < arrangementUser.Count; i++)
            {
                int? a = arrangementUser.ElementAt(i).ArrangementId;
                relatedArrangement.Add(a);
            }
            return relatedArrangement;
        }
        public List<Record> GetRecordByWorkerDate(int? roomID, DateTime date)
        {
            var roomDevice = dbContext.Set<RoomDevice>().FirstOrDefault(x => x.RoomId == roomID && x.IsActive == true
            && x.StartDate <= date && x.EndDate >= date);
            var result = dbContext.Set<Record>().Where(x => x.DeviceId == roomDevice.DeviceId
            && x.RecordDateTime < date.AddDays(1) && x.RecordDateTime > date).ToList();
            return result;
        }

        public List<Record> GetRecordByListDevicenDate(List<int?> deviceID, DateTime date)
        {
            return dbContext.Set<Record>().Where(x => deviceID.Contains(x.DeviceId) && x.RecordDateTime > date &&
            x.RecordDateTime < date.AddDays(1)).ToList();
        }

        public List<Record> GetRecordByDeviceDate(int? deviceID, DateTime date)
        {
            return dbContext.Set<Record>().Where(x => x.DeviceId == deviceID && x.RecordDateTime > date
            && x.RecordDateTime < date.AddDays(1)).ToList();
        }
    }
}
