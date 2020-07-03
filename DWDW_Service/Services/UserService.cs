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
        public UserService(UnitOfWork unitOfWork, IUserRepository userRepository) : base(unitOfWork)
        {
            this.userRepository = userRepository;
        }

        public UserViewModel CreateUserAsync(UserCreateModel create)
        {
            UserViewModel result;
            //checkValid to created
            var user = userRepository.GetUserByUsername(create.UserName);

            if (user != null)
            {
                //Map userCreateModel => userEntity to insert to database
                var userEntity = create.ToEntity<User>();

                //set IsAtive = true
                userEntity.IsActive = true;

                //add user to database
                userRepository.AddAsync(userEntity);

                //get User to response
                var userResponse = userRepository.GetUserByUsername(user.UserName);

                //map User => UserViewModel to API
                result = userResponse.ToViewModel<UserViewModel>();

            }
            else
            {
                throw new BaseException(ErrorMessages.USERNAME_IS_EXISTED);
            }

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
            UserViewModel result;
            //check validation
            var user = userRepository.Find(userUpdate.UserId);
            if (user != null)
            {
                //Map UserModel to UserEntity
                user.UserName = userUpdate.UserName;
                user.RoleId = userUpdate.RoleId;
                user.Phone = userUpdate.Phone;
                user.DateOfBirth = userUpdate.DateOfBirth;
                user.Gender = userUpdate.Gender;

                //update user
                userRepository.Update(user);

                //Map UserEntity to UserViewModel
                result = user.ToViewModel<UserViewModel>();
            }
            else
            {
                throw new BaseException(ErrorMessages.USERID_IS_NOT_EXISTED);
            }


            return result;

        }

        public UserViewModel DeActiveUserByAdmin(int id)
        {
            UserViewModel result;
            //check validation
            var user = userRepository.Find(id);
            if(user != null)
            {
                user.IsActive = false;
                userRepository.Update(user);
                result = user.ToViewModel<UserViewModel>();
            }
            else
            {
                throw new BaseException(ErrorMessages.USERID_IS_NOT_EXISTED);
            }
         

            //return ViewModel
            return result;

        }

        //unfinished
        public IEnumerable<UserViewModel> GetUserFromLocationByAdmin(int locationId)
        {
            //check validated
            var locationRepo = this.unitOfWork.LocationRepository;
            if(locationRepo.Find(locationId) != null)
            {
                var arrangementRepo = this.unitOfWork.ArrangementRepository;
                //get User from Arrangement (UserLocation) with locationID
                var arrangements = arrangementRepo.GetArrangementFromLocation(locationId);
                var users = arrangements.Select(a => a.User);

                return users.Select(u => u.ToViewModel<UserViewModel>());
            }
            else
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_EXISTED);

            }

          
        }
    }
}
