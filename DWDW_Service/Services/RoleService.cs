using DWDW_API.Core.Constants;
using DWDW_API.Core.Entities;
using DWDW_API.Core.Infrastructure;
using DWDW_API.Core.ViewModels;
using DWDW_Service.Repositories;
using DWDW_Service.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DWDW_Service.Services
{
    public interface IRoleService : IBaseService<Role>
    {
        IEnumerable<RoleViewModel> GetAll();
        RoleViewModel CreateRole(RoleCreateModel role);
        RoleViewModel UpdateRole(RoleUpdateModel role);
        RoleViewModel UpdateRoleActive(RoleActiveModel role);
    }
    public class RoleService : BaseService<Role>, IRoleService
    {
        private readonly IRoleRepository roleRepository;

        public RoleService(UnitOfWork unitOfWork, IRoleRepository roleRepository) : base(unitOfWork)
        {
            this.roleRepository = roleRepository;
        }

        public IEnumerable<RoleViewModel> GetAll()
        {
            return roleRepository.GetAll().Select(x => x.ToViewModel<RoleViewModel>());
        }

        public RoleViewModel CreateRole(RoleCreateModel role)
        {
            //Check valid
            var result = new RoleViewModel();
            if (roleRepository.GetRoleByRolename(role.RoleName) == null)
            {
                roleRepository.Add(new Role()
                {
                    RoleName = role.RoleName,
                    IsActive = true
                });
                result = roleRepository.GetRoleByRolename(role.RoleName).ToViewModel<RoleViewModel>();
            }
            else
            {
                throw new BaseException(ErrorMessages.ROLE_IS_EXISTED);
            }
            return result;
        }

        public RoleViewModel UpdateRole(RoleUpdateModel role)
        {
            var result = new RoleViewModel();
            var updateRole = roleRepository.Find(role.RoleId);
            if (updateRole != null)
            {
                updateRole.RoleName = role.RoleName;

                roleRepository.Update(updateRole);
                result = updateRole.ToViewModel<RoleViewModel>();
            }
            else
            {
                throw new BaseException(ErrorMessages.ROLE_IS_NOT_EXISTED);
            }
            return result;
        }

        public RoleViewModel UpdateRoleActive(RoleActiveModel role)
        {
            var result = new RoleViewModel();
            var updateRoleActive = roleRepository.Find(role.RoleId);
            if (updateRoleActive != null)
            {
                updateRoleActive.IsActive = role.IsActive;
                roleRepository.Update(updateRoleActive);
                result = roleRepository.Find(role.RoleId).ToViewModel<RoleViewModel>();
                //result = updateRoleActive.ToViewModel<RoleViewModel>();
            }
            else
            {
                throw new BaseException(ErrorMessages.ROLE_IS_NOT_EXISTED);
            }

            return result;
        }
    }
}
