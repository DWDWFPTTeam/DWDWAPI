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
        bool CheckUserShift(int userID, int? ArrangementID);
    }
    public class ArrangementRepository : BaseRepository<Arrangement>, IArrangementRepository
    {
        public ArrangementRepository(DbContext dbContext) : base(dbContext)
        {

        }

        public IEnumerable<Arrangement> GetArrangementFromLocation(int locationId)
        {
            return Get(a => a.LocationId.Equals(locationId), null, "User");
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
    }
}
