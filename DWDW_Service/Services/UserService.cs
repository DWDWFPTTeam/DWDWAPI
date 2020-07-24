﻿using DWDW_API.Core.Constants;
using DWDW_API.Core.Entities;
using DWDW_API.Core.Infrastructure;
using DWDW_API.Core.ViewModels;
using DWDW_Service.Repositories;
using DWDW_Service.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWDW_Service.Services
{
    public interface IUserService : IBaseService<User>
    {
        UserViewModel CreateUserAsync(UserCreateModel user);
        Task<User> LoginAsync(string username, string password);
        List<UserGetAllViewModel> GetAllByAdmin();
        IEnumerable<User> GetAllAllowAnonymous();
        UserViewModel UpdateUser(UserUpdateModel userUpdate);
        UserViewModel DeActiveUserByAdmin(int id);
        IEnumerable<UserViewModel> GetUserFromLocationByAdmin(int locationId);
        IEnumerable<UserViewModel> GetUserFromLocationsByManager(int userId);
        IEnumerable<UserViewModel> GetWorkerFromLocationsByManager(int userId, int locationID);
        IEnumerable<UserViewModel> GetUserFromOneLocationByManager(int userId, int locationId);
        UserViewModel GetUserById(int userId);
        UserViewModel UpdateUserDeviceToken(int userID, string deviceToken);
        UserViewModel UpdatePersonalInfo(int userID, UserPersonalUpdateModel updateUser);
        ArrangementViewModel AssignUserToLocation(ArrangementReceivedViewModel arrangement);
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
            //chi fix != -> ==
            if (user == null)
            {
                //chi check role existed
                //check role is Existed
                var role = this.unitOfWork.RoleRepository.Find(create.RoleId);
                if (role == null)
                {
                    throw new BaseException(ErrorMessages.ROLE_IS_NOT_EXISTED);
                }

                //Map userCreateModel => userEntity to insert to database
                var userEntity = create.ToEntity<User>();

                //set IsAtive = true
                userEntity.IsActive = true;

                //add user to database
                userRepository.Add(userEntity);

                //get User to response
                var userResponse = userRepository.GetUserByUsername(create.UserName);

                //map User => UserViewModel to API
                result = userResponse.ToViewModel<UserViewModel>();

            }
            else
            {
                throw new BaseException(ErrorMessages.USERNAME_IS_EXISTED);
            }

            return result;

        }
        //chi them field tra ve
        public List<UserGetAllViewModel> GetAllByAdmin()
        {
            List<UserGetAllViewModel> list = new List<UserGetAllViewModel>();
            var users = userRepository.GetAll()
                .Select(x => x.ToViewModel<UserViewModel>()).ToList();

            var arrangementRepo = this.unitOfWork.ArrangementRepository;
            var roleRepo = this.unitOfWork.RoleRepository;
            UserGetAllViewModel user = new UserGetAllViewModel();
            foreach (var item in users)
            {
                var location = arrangementRepo.GetArrangementLocationOfUser(item.UserId);
                var role = roleRepo.GetRoleByID((int)item.RoleId);
                if (location!=null)
                {
                    user = new UserGetAllViewModel()
                    {
                        UserId = item.UserId,
                        UserName = item.UserName,
                        Phone = item.Phone,
                        DateOfBirth = item.DateOfBirth,
                        Gender = item.Gender,
                        DeviceToken = item.DeviceToken,
                        RoleId = item.RoleId,
                        RoleName = role.RoleName,
                        LocationId = location.LocationId,
                        LocationCode = location.LocationCode,
                        StartDate = location.StartDate,
                        EndDate = location.EndDate
                    };
                }
                else
                {
                    user = new UserGetAllViewModel()
                    {
                        UserId = item.UserId,
                        UserName = item.UserName,
                        Phone = item.Phone,
                        DateOfBirth = item.DateOfBirth,
                        Gender = item.Gender,
                        DeviceToken = item.DeviceToken,
                        RoleId = item.RoleId,
                        RoleName = role.RoleName,
                    };
                }
                list.Add(user);
            }
            return list;
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

        public UserViewModel DeActiveUserByAdmin(int userId)
        {
            UserViewModel result;
            //check validation
            var user = userRepository.Find(userId);
            if (user != null)
            {
                using (var transaction = unitOfWork.CreateTransaction())
                {
                    try
                    {
                        //DeActive all Arrangement of the user
                        var arrangementRepo = unitOfWork.ArrangementRepository;
                        var arrangements = arrangementRepo.GetArrangementOfUser(userId);
                        //Check If the user does not belong to any arrangements so we do not need to DeActive Arrangement
                        if (arrangements.Count() > 0)
                        {
                            foreach (var arrangement in arrangements)
                            {
                                arrangement.IsActive = false;
                                arrangementRepo.Update(arrangement);
                            }
                        }
                        //DeActive user
                        user.IsActive = false;
                        userRepository.Update(user);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {

                        transaction.Rollback();
                        throw e;
                    }

                }
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
            IEnumerable<UserViewModel> result = new List<UserViewModel>();
            //check validated
            var locationRepo = this.unitOfWork.LocationRepository;
            if (locationRepo.Find(locationId) != null)
            {
                var arrangementRepo = this.unitOfWork.ArrangementRepository;
                //get User from Arrangement (UserLocation) with locationID
                var arrangements = arrangementRepo.GetArrangementFromLocation(locationId);
                var users = arrangements.Select(a => a.User);

                result = users.Select(u => u.ToViewModel<UserViewModel>());
            }
            else
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_EXISTED);

            }
            return result;

        }
        public IEnumerable<UserViewModel> GetUserFromLocationsByManager(int userId)
        {
            IEnumerable<UserViewModel> result = new List<UserViewModel>();
            var arrangementRepository = this.unitOfWork.ArrangementRepository;
            //get Arrangement of Manager 
            var arrangementsOfManager = arrangementRepository.GetArrangementOfUser(userId);

            //get Location which the manager is manage
            var locations = arrangementsOfManager.Select(a => a.Location);

            //Get Users which is Worker and Manager who work in these locations
            var users = new List<User>();
            foreach (var location in locations)
            {
                var usersFromLocation = arrangementRepository.GetArrangementFromLocation(location.LocationId.Value)
                                                 .Select(a => a.User);
                users.AddRange(usersFromLocation);
            }
            //toViewModel
            result = users.Select(u => u.ToViewModel<UserViewModel>());
            return result;
        }

        public IEnumerable<UserViewModel> GetWorkerFromLocationsByManager(int userId, int locationID)
        {
            IEnumerable<UserViewModel> result = new List<UserViewModel>();
            var arrangementRepo = this.unitOfWork.ArrangementRepository;
            var arrangement = arrangementRepo.CheckLocationManager(userId,locationID);
            if (arrangement != null)
            {
                var worker = userRepository.GetWorkerFromLocation(locationID);
                result = worker.Select(a => a.ToViewModel<UserViewModel>());
            }
            else
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_BELONG_TO_MANAGER);
            }
            return result;
        }
        public IEnumerable<UserViewModel> GetUserFromOneLocationByManager(int userId, int locationId)
        {
            IEnumerable<UserViewModel> result = new List<UserViewModel>();
            var locationRepository = this.unitOfWork.LocationRepository;
            if (locationRepository.Find(locationId) != null)
            {
                var arrangementRepository = this.unitOfWork.ArrangementRepository;

                var arrangement = arrangementRepository.GetArrangementOfUserInThisLocation(userId, locationId);

                if (arrangement != null)
                {
                    var location = arrangement.Location;
                    var users = arrangementRepository.GetArrangementFromLocation(location.LocationId.Value)
                                                     .Select(a => a.User);
                    result = users.Select(u => u.ToViewModel<UserViewModel>());
                }
                else
                {
                    throw new BaseException(ErrorMessages.LOCATION_IS_NOT_BELONG_TO_MANAGER);
                }
            }
            else
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_EXISTED);
            }

            return result;
        }

        public UserViewModel GetUserById(int userId)
        {
            return userRepository.Find(userId).ToViewModel<UserViewModel>();
        }

        public UserViewModel UpdateUserDeviceToken(int userID, string deviceToken)
        {
            var result = new UserViewModel();
            var user = userRepository.Find(userID);
            if (user != null)
            {
                user.DeviceToken = deviceToken;
                userRepository.Update(user);
                result = user.ToViewModel<UserViewModel>();
            }
            else
            {
                throw new BaseException(ErrorMessages.USERID_IS_NOT_EXISTED);
            }
            return result;
        }

        public UserViewModel UpdatePersonalInfo(int userID, UserPersonalUpdateModel updateUser)
        {
            var result = new UserViewModel();
            var user = userRepository.Find(userID);
            if (user != null)
            {
                user.Password = updateUser.Password;
                user.Phone = updateUser.Phone;
                user.DateOfBirth = updateUser.DateOfBirth;
                user.Gender = updateUser.Gender;
                userRepository.Update(user);
                result = user.ToViewModel<UserViewModel>();
            }
            else
            {
                throw new BaseException(ErrorMessages.USERID_IS_NOT_EXISTED);
            }
            return result;
        }

        public ArrangementViewModel AssignUserToLocation(ArrangementReceivedViewModel arrangement)
        {
            var location = unitOfWork.LocationRepository.Find(arrangement.LocationId);
            if (location == null)
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_EXISTED);
            }
            var user = this.userRepository.Find(arrangement.UserId);
            if (user == null)
            {
                throw new BaseException(ErrorMessages.USERID_IS_NOT_EXISTED);
            }
            var arrangementEntity = arrangement.ToEntity<Arrangement>();
            arrangementEntity.IsActive = true;
            unitOfWork.ArrangementRepository.Add(arrangementEntity);
            return arrangementEntity.ToViewModel<ArrangementViewModel>();
        }
    }
}
