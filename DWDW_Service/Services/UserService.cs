using DWDW_API.Core.Entities;
using DWDW_API.Core.Infrastructure;
using DWDW_API.Core.ViewModels;
using DWDW_Service.Repositories;
using DWDW_Service.UnitOfWorks;
using DWDW_Service.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWDW_Service.Services
{
    public interface IUserService : IBaseService<User>
    {
        UserViewModel CreateUserAsync(UserCreateModel user);
        Task<User> LoginAsync(string username, string password);
        IEnumerable<UserViewModel> GetAllByAdmin();
        IEnumerable<User> GetAllAllowAnonymous();
        UserViewModel UpdateUser(UserUpdateModel userUpdate);
        UserViewModel DeActiveUserByAdmin(int id);
        IEnumerable<UserViewModel> GetUserFromLocationByAdmin(int locationId);
    }
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly UserValidation userValid;
        public UserService(UnitOfWork unitOfWork, IUserRepository userRepository) : base(unitOfWork)
        {
            this.userRepository = userRepository;
            this.userValid = new UserValidation(userRepository);
        }

        public UserViewModel CreateUserAsync(UserCreateModel user)
        {
            //checkValid to created
            userValid.IsUsernameExisted(user.UserName);


            //Map userCreateModel => userEntity to insert to database
            var userEntity = user.ToEntity<User>();

            //set IsAtive = true
            userEntity.IsActive = true;

            //add user to database
            userRepository.AddAsync(userEntity);

            //get User to response
            var userResponse = userRepository.GetUserByUsername(user.UserName);

            //map User => UserViewModel to API
            var result = userResponse.ToViewModel<UserViewModel>();

            return result;

        }

        public IEnumerable<UserViewModel> GetAllByAdmin()
        {
            return userRepository.GetAll().Select(x => x.ToViewModel<UserViewModel>());
        }


        //This function just for testing. DO NOT SEND ENTITY MODEL TO API
        public IEnumerable<User> GetAllAllowAnonymous()
        {
            return userRepository.GetAll();
        }

        public async Task<User> LoginAsync(string username, string password)
        {
            var user = await userRepository.GetUserByUsernamePassword(username, password);
            return user;
        }

        public UserViewModel UpdateUser(UserUpdateModel userUpdate)
        {
            //check validation
            userValid.IsValidToUpdate(userUpdate);

            //Map UserModel to UserEntity
            var userUpdateEntity = userUpdate.ToEntity<User>();
            userRepository.Update(userUpdateEntity);

            //Get User to response
            var userResponse = userRepository.Find(userUpdate.UserId);

            //Map UserEntity to UserViewModel
            var userViewModel = userUpdateEntity.ToViewModel<UserViewModel>();

            return userViewModel;

        }

        public UserViewModel DeActiveUserByAdmin(int id)
        {
            //check validation
            userValid.IsIdNotExisted(id);

            var deActiveEntity = userRepository.Find(id);
            deActiveEntity.IsActive = false;

            userRepository.Update(deActiveEntity);

            //return ViewModel
            return deActiveEntity.ToViewModel<UserViewModel>();

        }

        //unfinished
        public IEnumerable<UserViewModel> GetUserFromLocationByAdmin(int locationId)
        {
            //check validated
            var arrangementRepo = this.unitOfWork.ArrangementRepository;
            var arrangements = arrangementRepo.GetArrangementFromLocation(locationId);
            var users = arrangements.Select(a => a.User);

            return users.Select(u => u.ToViewModel<UserViewModel>());
        }
    }
}
