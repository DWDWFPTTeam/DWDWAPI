using DWDW_API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DWDW_Service.Repositories
{
    public interface IArrangementRepository: IBaseRepository<Arrangement>
    {
        IEnumerable<Arrangement> GetArrangementFromLocation(int locationId);
        //this function for MANAGER and WORKER
        IEnumerable<Arrangement> GetArrangementOfUser(int userId);
        Arrangement GetArrangementOfUserInThisLocation(int userId, int locationId);
        bool CheckUserShift(int userID, int? ArrangementID);
        List<Arrangement> DisableArrangementFromLocation(int locationId);
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
                if (ManagerLocation != null)
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
    }
}
