using DWDW_Service.Repositories;
using DWDW_Service.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_Service.Validation
{
    public class RoleValidation
    {
        private readonly IRoleRepository roleRepository;

        public RoleValidation(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }
    }
}
