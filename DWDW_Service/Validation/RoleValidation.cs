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
        private readonly UnitOfWork unitOfWorks;

        public RoleValidation(IRoleRepository roleRepository, UnitOfWork unitOfWorks)
        {
            this.roleRepository = roleRepository;
            this.unitOfWorks = unitOfWorks;
        }
    }
}
