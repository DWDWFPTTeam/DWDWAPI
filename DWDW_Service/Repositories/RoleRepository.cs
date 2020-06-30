using DWDW_API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_Service.Repositories
{
    public interface IRoleRepository : IBaseRepository<Role>
    {

    }
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(DbContext dbContext) : base(dbContext)
        {

        }

    }
    

}
