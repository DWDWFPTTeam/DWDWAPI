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
        List<int?> GetLocationByUser(int userID);
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

        public List<int?> GetLocationByUser(int userID)
        {
            var arrangementUser = dbContext.Set<Arrangement>().Where(x => x.UserId == userID && x.IsActive == true).ToList();
            List<int?> relatedLocation = new List<int?>();
            for (int i = 0; i < arrangementUser.Count; i++)
            {
                int? a = arrangementUser.ElementAt(i).LocationId;
                relatedLocation.Add(a);
            }
            return relatedLocation;
        }

        public List<Location> SearchByLocationCode(string locationCode)
        {
            return this.dbContext.Set<Location>()
                .Where(l => l.LocationCode.Contains(locationCode))
                .ToList();
        }
    }
}
