using DWDW_API.Core.Constants;
using DWDW_API.Core.Entities;
using DWDW_API.Core.Infrastructure;
using DWDW_API.Core.ViewModels;
using DWDW_Service.Repositories;
using DWDW_Service.UnitOfWorks;
using Hangfire;
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
        List<UserGetAllViewModel> GetAllActiveByAdmin();
        IEnumerable<User> GetAllAllowAnonymous();
        UserViewModel UpdateUser(UserUpdateModel userUpdate);
        UserViewModel DeActiveUserByAdmin(int id);
        UserViewModel ActiveUserByAdmin(UserActiveModel user);
        List<UserGetAllViewModel> GetUserFromLocationByAdmin(int locationId);
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
                        IsActive = item.IsActive,
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
                        IsActive = item.IsActive,
                        RoleId = item.RoleId,
                        RoleName = role.RoleName,
                    };
                }
                list.Add(user);
            }
            return list;
        }

        public List<UserGetAllViewModel> GetAllActiveByAdmin()
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
                if (location != null)
                {
                    user = new UserGetAllViewModel()
                    {
                        UserId = item.UserId,
                        UserName = item.UserName,
                        Phone = item.Phone,
                        DateOfBirth = item.DateOfBirth,
                        Gender = item.Gender,
                        DeviceToken = item.DeviceToken,
                        IsActive = item.IsActive,
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
                        IsActive = item.IsActive,
                        RoleId = item.RoleId,
                        RoleName = role.RoleName,
                    };
                }
                list.Add(user);
            }
            var listFilterActive = list.Where(x => x.IsActive == true).ToList();
            return listFilterActive;
        }


        //This function just for testing. DO NOT SEND ENTITY MODEL TO API
        public IEnumerable<User> GetAllAllowAnonymous()
        {
            return userRepository.GetAll();
        }

        public async Task<User> LoginAsync(string username, string password)
        {
            var roomDeviceRepo = unitOfWork.RoomDeviceRepository;
            var arrangementRepo = unitOfWork.ArrangementRepository;

            var user = await userRepository.GetUserByUsernamePassword(username, password);
            if(user == null)
            {
                throw new BaseException(ErrorMessages.INVALID_USERNAME_PASSWORD);
            }
            var overdueRoomDevice = roomDeviceRepo.GetOverdue();
            var overdueArrangement = arrangementRepo.GetOverdue();
            RecurringJob.AddOrUpdate("DeactiveOverdue", () => DeactiveOverdue(overdueRoomDevice, overdueArrangement), "0 0 * * *", TimeZoneInfo.Local);

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
        public UserViewModel ActiveUserByAdmin(UserActiveModel userActive)
        {
            UserViewModel result;
            //check validation
            var user = userRepository.Find(userActive.UserId);
            if (user != null)
            {
                if (userActive.IsActive == true)
                {
                    user.IsActive = true;
                    userRepository.Update(user);
                    result = user.ToViewModel<UserViewModel>();
                }
                else
                {
                    using (var transaction = unitOfWork.CreateTransaction())
                    {
                        try
                        {
                            //DeActive all Arrangement of the user
                            var arrangementRepo = unitOfWork.ArrangementRepository;
                            var arrangements = arrangementRepo.GetArrangementOfUser(userActive.UserId);
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
                
            }
            //xx
            else
            {
                throw new BaseException(ErrorMessages.USERID_IS_NOT_EXISTED);
            }
            return result;
        }


        //unfinished
        public List<UserGetAllViewModel> GetUserFromLocationByAdmin(int locationId)
        {
            //IEnumerable<UserViewModel> result = new List<UserViewModel>();
            List<UserGetAllViewModel> list = new List<UserGetAllViewModel>();
            UserGetAllViewModel user = new UserGetAllViewModel();
            //check validated
            var locationRepo = this.unitOfWork.LocationRepository;
            var location = locationRepo.Find(locationId);
            if (location != null)
            {
                var arrangementRepo = this.unitOfWork.ArrangementRepository;
                var roleRepo = this.unitOfWork.RoleRepository;
                //get User from Arrangement (UserLocation) with locationID
                var arrangements = arrangementRepo.GetArrangementUserFromLocation(locationId);
                foreach (var item in arrangements)
                {
                    Role role = new Role();
                    var userDetail = userRepository.Find(item.UserId);
                    if (userDetail!=null)
                    {
                        role = roleRepo.Find(userDetail.RoleId);
                    }
                    user = new UserGetAllViewModel()
                    {
                        UserId = item.UserId,
                        UserName = userDetail.UserName,
                        Phone = userDetail.Phone,
                        DateOfBirth = userDetail.DateOfBirth,
                        Gender = userDetail.Gender,
                        DeviceToken = userDetail.DeviceToken,
                        IsActive = userDetail.IsActive,
                        RoleId = userDetail.RoleId,
                        RoleName = role.RoleName,
                        LocationId = location.LocationId,
                        LocationCode = location.LocationCode,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate
                    };
                    list.Add(user);
                }
            }
            else
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_EXISTED);
            }
            return list;

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
            var arrangement = arrangementRepo.CheckLocationManagerWorker(userId,locationID);
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

        public void DeactiveOverdue(List<RoomDevice> roomDeviceOverdue, List<Arrangement> arrangementOverdue)
        {
            var roomDeviceRepo = unitOfWork.RoomDeviceRepository;
            var arrangementRepo = unitOfWork.ArrangementRepository;

            foreach (var element in roomDeviceOverdue)
            {
                element.IsActive = false;
                roomDeviceRepo.Update(element);
            }

            foreach (var attribute in arrangementOverdue)
            {
                attribute.IsActive = false;
                arrangementRepo.Update(attribute);
            }

        }
    }
}
