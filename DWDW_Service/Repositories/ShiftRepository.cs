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
    public interface IShiftRepository : IBaseRepository<Shift>
    {
        List<Shift> GetShiftByDate(DateTime date);
        Shift GetLatest();
        void DisableShiftsByArrangementId(int? arrangementId);
        void DisableOldSameShift(int? arrangementID, int? shiftRoomId, DateTime? shiftDate);
        List<Shift> GetShiftSubAccount(List<int?> arrangementID);
        Shift GetShiftByRoomDate(int? roomId, DateTime? recordDateTime);
        IEnumerable<ShiftViewModel> GetShiftFromLocation(int locationID);
        IEnumerable<ShiftViewModel> GetShiftFromLocationWorker(int userID, int locationID);
        string GetRoomCode(int? roomID);
        string GetUsername(int? arrangementID);
        int? GetWorkerID(int? arrangementID);
    }
    public class ShiftRepository : BaseRepository<Shift>, IShiftRepository
    {
        public ShiftRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public List<Shift> GetShiftByDate(DateTime date)
        {

            return this.dbContext.Set<Shift>().Where(x => x.Date == date.Date).ToList();
        }

        public Shift GetLatest()
        {
            return this.dbContext.Set<Shift>().OrderByDescending(x => x.ShiftId).First();
        }

        public void DisableOldSameShift(int? arrangementID, int? shiftRoomId, DateTime? shiftDate)
        {
            var OldShift = dbContext.Set<Shift>().Where(x => x.ArrangementId == arrangementID
                                                 && x.RoomId == shiftRoomId && x.Date == shiftDate)
                                                 .ToList();
            OldShift.ForEach(a => a.IsActive = false);
            dbContext.SaveChanges();
        }

        public void DisableShiftsByArrangementId(int? arrangementId)
        {
            var shifts = this.dbContext.Set<Shift>()
                .Where(s => s.ArrangementId == arrangementId)
                .ToList();
            shifts.ForEach(s => s.IsActive = false);
            this.dbContext.SaveChanges();
        }

        public List<Shift> GetShiftSubAccount(List<int?> arrangementID)
        {
            return dbContext.Set<Shift>().Where(x => arrangementID.Contains(x.ArrangementId)).ToList();
        }

        public Shift GetShiftByRoomDate(int? roomId, DateTime? recordDateTime)
        {

            return Get(s => s.RoomId == roomId.Value 
                       && s.Date.Value.CompareTo(recordDateTime.Value.Date) == 0, null, "Arrangement")
                       .FirstOrDefault();
        }

        public IEnumerable<ShiftViewModel> GetShiftFromLocation(int locationID)
        {
            IEnumerable<ShiftViewModel> result = new List<ShiftViewModel>();
            //var result = new List<Shift>();
            var arrangement = dbContext.Set<Arrangement>().Where(x => x.LocationId == locationID && x.IsActive == true).ToList();
            List<int?> arrangementID = new List<int?>();
            for(int i = 0; i < arrangement.Count; i++)
            {
                int? a = arrangement.ElementAt(i).ArrangementId;
                arrangementID.Add(a);
            }
            var shiftLocation = dbContext.Set<Shift>().Where(x => arrangementID.Contains(x.ArrangementId)).ToList();
            result = shiftLocation.Select(x => x.ToViewModel<ShiftViewModel>());
            return result;
        }

        public IEnumerable<ShiftViewModel> GetShiftFromLocationWorker(int userID, int locationID)
        {
            IEnumerable<ShiftViewModel> result = new List<ShiftViewModel>();
            //var result = new List<Shift>();
            var arrangement = dbContext.Set<Arrangement>().Where(x => x.LocationId == locationID && x.UserId == userID
            && x.IsActive == true).ToList();
            List<int?> arrangementID = new List<int?>();
            for (int i = 0; i < arrangement.Count; i++)
            {
                int? a = arrangement.ElementAt(i).ArrangementId;
                arrangementID.Add(a);
            }
            var shiftLocation = dbContext.Set<Shift>().Where(x => arrangementID.Contains(x.ArrangementId)).ToList();
            result = shiftLocation.Select(x => x.ToViewModel<ShiftViewModel>());
            return result;
        }

        public string GetRoomCode(int? roomID)
        {
            string result = "";
            var room = dbContext.Set<Room>().Find(roomID);
            if (room != null)
            {
                result = room.RoomCode;
            }
            return result;
        }
        public string GetUsername(int? arrangementID)
        {
            string result = "";
            var arrangement = dbContext.Set<Arrangement>().FirstOrDefault(x => x.ArrangementId == arrangementID && x.IsActive == true);
            if (arrangement != null)
            {
                var user = dbContext.Set<User>().Find(arrangement.UserId);
                result = user.UserName;
            }
            return result;
        }
        public int? GetWorkerID(int? arrangementID)
        {
            int? result = null;
            var arrangement = dbContext.Set<Arrangement>().FirstOrDefault(x => x.ArrangementId == arrangementID && x.IsActive == true);
            if (arrangement != null)
            {
                var user = dbContext.Set<User>().Find(arrangement.UserId);
                result = user.UserId;
            }
            return result;
        }
    }
}
