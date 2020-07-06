using DWDW_API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DWDW_Service.Repositories
{
    public interface ILocationRepository : IBaseRepository<Location>
    {
        Location GetLocationByLocationCode(string locationCode);
        List<Location> SearchByLocationCode(string locationCode);
    }
    public class LocationRepository : BaseRepository<Location>, ILocationRepository
    {
        public LocationRepository(DbContext dbContext) : base(dbContext)
        {

        }

        public Location GetLocationByLocationCode(string locationCode)
        {
            return this.dbContext.Set<Location>().FirstOrDefault
                 (l => l.LocationCode .Trim().ToLower().Equals(locationCode.Trim().ToLower()));
        }

        public List<Location> SearchByLocationCode(string locationCode)
        {
            return this.dbContext.Set<Location>()
                .Where(l => l.LocationCode.Contains(locationCode))
                .ToList();
        }
    }
}
