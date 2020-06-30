using DWDW_API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_Service.Repositories
{
    public interface ILocationRepository : IBaseRepository<Location>
    {

    }
    public class LocationRepository : BaseRepository<Location>, ILocationRepository
    {
        public LocationRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
