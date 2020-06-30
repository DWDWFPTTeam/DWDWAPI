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
        IEnumerable<UserViewModel> GetAll();
        IEnumerable<User> GetAllAllowAnonymous();
    }
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IUserRepository userRepository;
        public UserService(UnitOfWork unitOfWork, IUserRepository userRepository) : base(unitOfWork)
        {
            this.userRepository = userRepository;
        }

        public UserViewModel CreateUserAsync(UserCreateModel user)
        {
            //checkValid to created
            var userValid = new UserValidation(this.userRepository);
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

        public IEnumerable<UserViewModel> GetAll()
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
    }
}
