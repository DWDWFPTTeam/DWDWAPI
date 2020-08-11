using DWDW_API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DWDW_Service.Repositories
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Role GetRoleByRolename(string roleName);
        Role GetRoleByID(int id);
    }
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(DbContext dbContext) : base(dbContext)
        {

        }

        public Role GetRoleByRolename(string roleName)
        {
            return this.dbContext.Set<Role>().FirstOrDefault(x => x.RoleName.Trim().ToLower()
                                                            .Equals(roleName.Trim().ToLower()));
        }

        public Role GetRoleByID(int id)
        {
            return this.dbContext.Set<Role>().FirstOrDefault(x => x.RoleId == id);
        }
    }
    

}
