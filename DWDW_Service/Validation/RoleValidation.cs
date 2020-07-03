using DWDW_API.Core.Constants;
using DWDW_API.Core.Entities;
using DWDW_API.Core.Infrastructure;
using DWDW_API.Core.ViewModels;
using DWDW_Service.Repositories;
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

        public void IsRolenameExisted(string roleName)
        {
            if (roleRepository.GetRoleByRolename(roleName) != null)
            {
                throw new BaseException(ErrorMessages.ROLE_IS_EXISTED);
            }
        }
        
        public void IsRoleNotExisted(int roleId)
        {
            if(roleRepository.Find(roleId) == null)
            {
                throw new BaseException(ErrorMessages.ROLE_IS_NOT_EXISTED);
            }
        }
    }
}
