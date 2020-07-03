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
    }
}
