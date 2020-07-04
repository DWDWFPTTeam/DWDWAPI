using DWDW_API.Core.Entities;
using DWDW_API.Core.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DWDW_Service.Repositories
{
    public interface IShiftRepository : IBaseRepository<Shift>
    {
        List<Shift> GetShiftByDate(DateTime date);
        Shift GetLatest();
        void DisableShiftsByArrangementId(int? arrangementId);
        void DisableOldSameShift(ShiftCreateModel shift);
        List<Shift> GetShiftSubAccount(List<int?> arrangementID);


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

        public void DisableOldSameShift(ShiftCreateModel shift)
        {
            var OldShift = dbContext.Set<Shift>().Where(x => x.ArrangementId == shift.ArrangementId
                && x.RoomId == shift.RoomId && x.Date == shift.Date
                && x.ShiftType == shift.ShiftType).ToList();
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
    }

}
