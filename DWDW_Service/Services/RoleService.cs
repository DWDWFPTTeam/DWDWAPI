using DWDW_API.Core.Entities;
using DWDW_Service.Repositories;
using DWDW_Service.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_Service.Services
{
    public interface IRoleService : IBaseService<Role>
    {

    }
    public class RoleService : BaseService<Role>, IRoleService
    {
        private readonly IRoleRepository roleRepository;
        public RoleService(UnitOfWork unitOfWork, IRoleRepository roleRepository) : base(unitOfWork)
        {
            this.roleRepository = roleRepository;
        }


    }
}
