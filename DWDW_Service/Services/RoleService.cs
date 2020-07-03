using DWDW_API.Core.Entities;
using DWDW_API.Core.ViewModels;
using DWDW_Service.Repositories;
using DWDW_Service.UnitOfWorks;
using DWDW_Service.Validation;
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
        RoleViewModel UpdateRoleActive(RoleActiveModel role);
    }
    public class RoleService : BaseService<Role>, IRoleService
    {
        private readonly IRoleRepository roleRepository;
        private readonly RoleValidation roleValidation;

        public RoleService(UnitOfWork unitOfWork, IRoleRepository roleRepository) : base(unitOfWork)
        {
            this.roleRepository = roleRepository;
            this.roleValidation = new RoleValidation(roleRepository);
        }

        public IEnumerable<RoleViewModel> GetAll()
        {
            return roleRepository.GetAll().Select(x => x.ToViewModel<RoleViewModel>());
        }

        public RoleViewModel CreateRole(RoleCreateModel role)
        {
            //Check valid
            roleValidation.IsRolenameExisted(role.RoleName);

            //Map model to entity
            var roleEntity = role.ToEntity<Role>();

            roleEntity.IsActive = true;
            roleRepository.AddAsync(roleEntity);

            var roleResponse = roleRepository.GetRoleByRolename(role.RoleName);

            var result = roleResponse.ToViewModel<RoleViewModel>();
            return result;
        }

        public RoleViewModel UpdateRoleActive(RoleActiveModel role)
        {
            roleValidation.QualifyToUpdate(role);

            var roleEntity = role.ToEntity<Role>();

            //roleEntity.IsActive = role.IsActive;
            roleRepository.Update(roleEntity);

            var roleResponse = roleRepository.Find(role.RoleId);
            var result = roleResponse.ToViewModel<RoleViewModel>();
            return result;
        }
    }
}
