﻿using DWDW_API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DWDW_Service.Repositories
{
    public interface IArrangementRepository : IBaseRepository<Arrangement>
    {
        IEnumerable<Arrangement> GetArrangementFromLocation(int locationId);
        //this function for MANAGER and WORKER
        IEnumerable<Arrangement> GetArrangementOfUser(int userId);
        Arrangement GetArrangementOfUserInThisLocation(int userId, int locationId);
        bool CheckUserShift(int userID, int? ArrangementID);
        List<Arrangement> DisableArrangementFromLocation(int locationId);
        List<int?> GetArrangementBelongToManager(int userID);
        List<int?> GetArrangementBelongToWorker(int userID);
    }
    public class ArrangementRepository : BaseRepository<Arrangement>, IArrangementRepository
    {
        public ArrangementRepository(DbContext dbContext) : base(dbContext)
        {

        }

        public IEnumerable<Arrangement> GetArrangementFromLocation(int locationId)
        {
            return Get(a => a.LocationId.Equals(locationId) && a.IsActive == true, null, "User");
        }

        public IEnumerable<Arrangement> GetArrangementOfUser(int userId)
        {
            return Get(a => a.UserId.Equals(userId) && a.IsActive == true, null, "Location");
        }

        public Arrangement GetArrangementOfUserInThisLocation(int userId, int locationId)
        {
            return Get(a => a.UserId.Equals(userId)
                       && a.LocationId.Equals(locationId)
                       && a.IsActive == true, null, "Location").FirstOrDefault();
        }
        public bool CheckUserShift(int userID, int? ArrangementID)
        {
            bool result = false;
            var arrangement = this.dbContext.Set<Arrangement>().Find(ArrangementID);
            if (arrangement != null)
            {
                var ManagerLocation = this.dbContext.Set<Arrangement>().FirstOrDefault(
                                x => x.LocationId == arrangement.LocationId
                                && x.UserId == userID);
                //Khi moi quan he ton tai va duoc su dung thi moi duoc set shift
                if (ManagerLocation != null && ManagerLocation.IsActive == true)
                {
                    result = true;
                }
            }
            return result;
        }

        public List<Arrangement> DisableArrangementFromLocation(int locationId)
        {
            var arrangements = this.dbContext.Set<Arrangement>()
               .Where(a => a.LocationId == locationId).ToList();
            arrangements.ForEach(a => a.IsActive = false);
            this.dbContext.SaveChanges();
            return arrangements;
        }
        public List<int?> GetArrangementBelongToManager(int userID)
        {
            var arrangementManager = dbContext.Set<Arrangement>().Where(x => x.UserId == userID).ToList();
            List<int?> qualifyLocation = new List<int?>();
            List<int?> qualifyUserRelatedLocation = new List<int?>();
            //Lay ra danh sach nhung location thuoc ve manager
            for (int i = 0; i < arrangementManager.Count; i++)
            {
                int? a = arrangementManager.ElementAt(i).LocationId;
                qualifyLocation.Add(a);
            }
            //Lay ra nhung arrangement co location duoi quyen manager
            var arrangementRelated = dbContext.Set<Arrangement>().Where(x => qualifyLocation.Contains(x.LocationId)).ToList();
            for (int i = 0; i < arrangementRelated.Count; i++)
            {
                int? a = arrangementRelated.ElementAt(i).ArrangementId;
                qualifyUserRelatedLocation.Add(a);
            }
            return qualifyUserRelatedLocation;
        }

        public List<int?> GetArrangementBelongToWorker(int userID)
        {
            var arrangementManager = dbContext.Set<Arrangement>().Where(x => x.UserId == userID).ToList();

            List<int?> qualifyWorkerRelated = new List<int?>();
            //Lay ra danh sach nhung arrangement thuoc ve user
            for (int i = 0; i < arrangementManager.Count; i++)
            {
                int? a = arrangementManager.ElementAt(i).ArrangementId;
                qualifyWorkerRelated.Add(a);
            }

            return qualifyWorkerRelated;
        }
    }
}
