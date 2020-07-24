﻿using DWDW_API.Core.Entities;
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
        void DisableOldSameShift(int? arrangementID, ShiftCreateModel shift);
        List<Shift> GetShiftSubAccount(List<int?> arrangementID);
        Shift GetShiftByRoomDate(int? roomId, DateTime? recordDateTime);
        IEnumerable<ShiftViewModel> GetShiftFromLocation(int locationID);
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

        public void DisableOldSameShift(int? arrangementID, ShiftCreateModel shift)
        {
            var OldShift = dbContext.Set<Shift>().Where(x => x.ArrangementId == arrangementID
                                                 && x.RoomId == shift.RoomId && x.Date == shift.Date)
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
    }
}
