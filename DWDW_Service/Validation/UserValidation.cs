using DWDW_API.Core.Constants;
using DWDW_API.Core.Infrastructure;
using DWDW_Service.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_Service.Validation
{
    public class UserValidation
    {
        private readonly IUserRepository userRepository;

        public UserValidation(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        
        public void IsExisted(int id)
        {
            if (userRepository.Find(id) != null)
            {
                throw new BaseException(ErrorMessages.USERID_IS_EXISTED);
            }
        }
        public void IsNotExisted(int id)
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
