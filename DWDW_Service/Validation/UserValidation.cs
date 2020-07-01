using DWDW_API.Core.Constants;
using DWDW_API.Core.Infrastructure;
using DWDW_API.Core.ViewModels;
using DWDW_Service.Repositories;
using DWDW_Service.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_Service.Validation
{
    public class UserValidation
    {
        private readonly IUserRepository userRepository;
        private readonly UnitOfWork unitOfWorks;

        public UserValidation(IUserRepository userRepository, UnitOfWork unitOfWorks)
        {
            this.userRepository = userRepository;
            this.unitOfWorks = unitOfWorks;
        }

        public void IsValidToUpdate(UserUpdateModel user)
        {
            this.IsIdNotExisted(user.UserId);
        }
        
        public void IsIdExisted(int id)
        {
            if (userRepository.Find(id) != null)
            {
                throw new BaseException(ErrorMessages.USERID_IS_EXISTED);
            }
        }
        public void IsIdNotExisted(int id)
        {
            if(userRepository.Find(id) == null)
            {
                throw new BaseException(ErrorMessages.USERID_IS_NOT_EXISTED);
            }

        }

        public void IsUsernameExisted(string username)
        {
            if(userRepository.GetUserByUsername(username) != null)
            {
                throw new BaseException(ErrorMessages.USERNAME_IS_EXISTED);
            }
        }
    }
}
