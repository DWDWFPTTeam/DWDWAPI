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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DWDW_Service.Services
{
    public interface IUserService : IBaseService<User>
    {
        UserViewModel CreateUser(UserCreateModel user);
        User LoginAsync(string username, string password);
        //List<UserGetAllViewModel> GetAllByAdmin(int userId);
        //refactor by dat
        IEnumerable<UserGetAllViewModel> GetAllByAdmin();
        UserGetAllViewModel GetByIDAdmin(int userId);
        //List<UserGetAllViewModel> GetAllActiveByAdmin();
        //refactor by dat
        IEnumerable<UserGetAllViewModel> GetAllActiveByAdmin(int userId);
        IEnumerable<User> GetAllAllowAnonymous();
        UserViewModel UpdateUser(UserUpdateModel userUpdate);
        UserViewModel DeActiveUserByAdmin(int id);
        UserViewModel ActiveUserByAdmin(UserActiveModel userActive);
        IEnumerable<UserGetAllViewModel> GetUserFromLocationByAdmin(int locationId1);
        IEnumerable<UserViewModel> GetUserFromLocationsByManager(int userId);
        IEnumerable<UserViewModel> GetWorkerFromLocationsByManager(int userId, int locationID);
        IEnumerable<UserViewModel> GetUserFromOneLocationByManager(int userId, int locationId);
        UserViewModel GetUserById(int userId);
        UserViewModel UpdateUserDeviceToken(int userID, string deviceToken);
        UserViewModel UpdatePersonalInfo(int userID, UserPersonalUpdateModel updateUser);
        ArrangementViewModel AssignUserToLocation(ArrangementReceivedViewModel arrangement);
        ArrangementViewModel DeassignUserToLocation(ArrangementDisableViewModel arrangement);
        void DeactiveOverdue();
    }
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IUserRepository userRepository;
        public UserService(UnitOfWork unitOfWork, IUserRepository userRepository) : base(unitOfWork)
        {
            this.userRepository = userRepository;
        }

        public UserViewModel CreateUser(UserCreateModel create)
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

        public IEnumerable<UserGetAllViewModel> GetAllByAdmin()
        {

            var users = userRepository.GetAll().Select(user =>
            {
                var userGetAllViewModel = user.ToViewModel<UserGetAllViewModel>();
                userGetAllViewModel.Locations = this.unitOfWork.ArrangementRepository.Get(arr => arr.UserId == user.UserId
                                                                        && arr.IsActive == true, null, "Location")
                                                                        .Select(arr =>
                                                                        {
                                                                            var location = arr.Location.ToViewModel<LocationUserViewModel>();
                                                                            location.StartDate = arr.StartDate;
                                                                            location.EndDate = arr.EndDate;
                                                                            return location;
                                                                        });
                userGetAllViewModel.Role = this.unitOfWork.RoleRepository.Get(role => role.RoleId == user.RoleId
                                                                              && role.IsActive == true, null, "")
                                                                              .FirstOrDefault().ToViewModel<RoleViewModel>();
                return userGetAllViewModel;
            });

            return users;
        }


        public UserGetAllViewModel GetByIDAdmin(int userId)
        {
            UserGetAllViewModel result;
            var user = userRepository.Find(userId);
            if (user == null)
            {
                throw new BaseException(ErrorMessages.USERID_IS_NOT_EXISTED);
            }
            result = user.ToViewModel<UserGetAllViewModel>();
            result.Locations = this.unitOfWork.ArrangementRepository.Get(arr => arr.UserId == user.UserId
                                                                        && arr.IsActive == true, null, "Location")
                                                                        .Select(arr => arr.Location.ToViewModel<LocationUserViewModel>());
            result.Role = this.unitOfWork.RoleRepository.Get(role => role.RoleId == user.RoleId
                                                                          && role.IsActive == true, null, "")
                                                                          .FirstOrDefault().ToViewModel<RoleViewModel>();
            return result;
        }



        public IEnumerable<UserGetAllViewModel> GetAllActiveByAdmin(int userId)
        {
            var user = userRepository.Find(userId);
            if (user == null)
            {
                throw new BaseException(ErrorMessages.USERID_IS_NOT_EXISTED);
            }
            if (user.RoleId.Value != int.Parse(Constant.ADMIN))
            {
                throw new BaseException(ErrorMessages.INVALID_ROLE_FOR_THIS_ACTION);
            }
            var users = userRepository.Get(u => u.IsActive == true).Select(user =>
            {
                var userGetAllViewModel = user.ToViewModel<UserGetAllViewModel>();
                userGetAllViewModel.Locations = this.unitOfWork.ArrangementRepository.Get(arr => arr.UserId == user.UserId
                                                                       && arr.IsActive == true, null, "Location")
                                                                       .Select(arr =>
                                                                       {
                                                                           var location = arr.Location.ToViewModel<LocationUserViewModel>();
                                                                           location.StartDate = arr.StartDate;
                                                                           location.EndDate = arr.EndDate;
                                                                           return location;
                                                                       });
                userGetAllViewModel.Role = this.unitOfWork.RoleRepository.Get(role => role.RoleId == user.RoleId
                                                                             && role.IsActive == true, null, "")
                                                                             .FirstOrDefault().ToViewModel<RoleViewModel>();
                return userGetAllViewModel;
            });

            return users;
        }



        //This function just for testing. DO NOT SEND ENTITY MODEL TO API

        public IEnumerable<User> GetAllAllowAnonymous()
        {
            return userRepository.GetAll();
        }

        public User LoginAsync(string username, string password)
        {
            var roomDeviceRepo = unitOfWork.RoomDeviceRepository;
            var arrangementRepo = unitOfWork.ArrangementRepository;

            var user = userRepository.GetUserByUsernamePassword(username, password);
            if (user == null)
            {
                throw new BaseException(ErrorMessages.INVALID_USERNAME_PASSWORD);
            }
            //var overdueRoomDevice = roomDeviceRepo.GetOverdue();
            //var overdueArrangement = arrangementRepo.GetOverdue();
            //RecurringJob.AddOrUpdate("DeactiveOverdue", () => DeactiveOverdue(), "0 0 * * *", TimeZoneInfo.Local);

            return user;
        }


        public UserViewModel UpdateUser(UserUpdateModel userUpdate)
        {
            //check validation
            var user = userRepository.Find(userUpdate.UserId);
            if (user == null)
            {
                throw new BaseException(ErrorMessages.USER_IS_NOT_EXISTED);
            }
            user.FullName = userUpdate.FullName;
            user.DateOfBirth = userUpdate.DateOfBirth;
            user.Gender = userUpdate.Gender;
            user.Phone = userUpdate.Phone;
            user.RoleId = userUpdate.RoleId;
            //update user
            userRepository.Update(user);

            return user.ToViewModel<UserViewModel>();

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
        public IEnumerable<UserGetAllViewModel> GetUserFromLocationByAdmin(int locationId)
        {

            var location = this.unitOfWork.LocationRepository.Find(locationId);
            if (location == null)
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_EXISTED);
            }
            var users = userRepository.GetUserFromLocation(locationId);
            var result = users.Select(x => x.ToViewModel<UserGetAllViewModel>()).ToList();
            foreach (var element in result)
            {
                element.Locations = this.unitOfWork.ArrangementRepository.Get(arr => arr.UserId == element.UserId
                                                                   && arr.IsActive == true, null, "Location")
                                                                   .Select(arr =>
                                                                   {
                                                                       var location = arr.Location.ToViewModel<LocationUserViewModel>();
                                                                       location.StartDate = arr.StartDate;
                                                                       location.EndDate = arr.EndDate;
                                                                       return location;
                                                                   });
                element.Role = this.unitOfWork.RoleRepository.Get(role => role.RoleId == element.RoleId
                                                                         && role.IsActive == true, null, "")
                                                                         .FirstOrDefault().ToViewModel<RoleViewModel>();

            }
            return result;
            //var users = this.unitOfWork.ArrangementRepository.Get(arr => arr.LocationId == locationId
            //                                                    && arr.IsActive == true, null, "User").Select(arr => arr.User);
            //return users.Select(u =>
            //{
            //    var userGetAllViewModel = user.ToViewModel<UserGetAllViewModel>();
            //userGetAllViewModel.Locations = this.unitOfWork.ArrangementRepository.Get(arr => arr.UserId == user.UserId
            //                                                       && arr.IsActive == true, null, "Location")
            //                                                       .Select(arr =>
            //                                                       {
            //                                                           var location = arr.Location.ToViewModel<LocationUserViewModel>();
            //                                                           location.StartDate = arr.StartDate;
            //                                                           location.EndDate = arr.EndDate;
            //                                                           return location;
            //                                                       });
            //userGetAllViewModel.Role = this.unitOfWork.RoleRepository.Get(role => role.RoleId == user.RoleId
            //                                                             && role.IsActive == true, null, "")
            //                                                             .FirstOrDefault().ToViewModel<RoleViewModel>();
            //    return userGetAllViewModel;
            //});


        }

        public IEnumerable<UserViewModel> GetWorkerFromLocationsByManager(int userId, int locationID)
        {
            IEnumerable<UserViewModel> result = new List<UserViewModel>();
            var arrangementRepo = this.unitOfWork.ArrangementRepository;
            var arrangement = arrangementRepo.CheckLocationManagerWorker(userId, locationID, DateTime.Now);
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
            var user = userRepository.Find(userId);
            if (user == null)
            {
                throw new BaseException(ErrorMessages.USERID_IS_NOT_EXISTED);
            }
            return user.ToViewModel<UserViewModel>();
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
            if (user == null)
            {
                throw new BaseException(ErrorMessages.USERID_IS_NOT_EXISTED);
            }
            user.FullName = updateUser.FullName;
            user.Phone = updateUser.Phone;
            user.DateOfBirth = updateUser.DateOfBirth;
            user.Gender = updateUser.Gender;
            userRepository.Update(user);
            result = user.ToViewModel<UserViewModel>();

            return result;
        }

        public ArrangementViewModel AssignUserToLocation(ArrangementReceivedViewModel arrangement)
        {
            var result = new ArrangementViewModel();
            var arrangementRepo = unitOfWork.ArrangementRepository;
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
            if (arrangement.StartDate < DateTime.Now)
            {
                throw new BaseException(ErrorMessages.STARDATE_MUST_BIGGER_NOW);
            }
            if (arrangement.StartDate > arrangement.EndDate)
            {
                throw new BaseException(ErrorMessages.DATE_INVALID);
            }
            //Lay ra arrangement moi nhat da ton tai cua user
            var existedArrangement = arrangementRepo.GetArrangementOfUserInThisLocation(arrangement.UserId, arrangement.LocationId);
            DateTime oldArrangementEnd = existedArrangement.EndDate ?? DateTime.Now;
            //Worker có tồn tại mối quan hệ cũ
            if (existedArrangement != null && user.RoleId != 2)
            {
                if (arrangement.StartDate <= oldArrangementEnd.AddDays(-1))
                {
                    throw new BaseException(ErrorMessages.STARDATE_MUST_BIGGER_NOW);
                }
                var arrangementEntity = arrangement.ToEntity<Arrangement>();
                arrangementEntity.IsActive = true;
                unitOfWork.ArrangementRepository.Add(arrangementEntity);
                result = arrangementEntity.ToViewModel<ArrangementViewModel>();
            }
            //Worker không tồn tại mối quan hệ cũ
            else if (existedArrangement == null && user.RoleId != 2)
            {
                var arrangementEntity = arrangement.ToEntity<Arrangement>();
                arrangementEntity.IsActive = true;
                unitOfWork.ArrangementRepository.Add(arrangementEntity);
                result = arrangementEntity.ToViewModel<ArrangementViewModel>();
            }
            //Manager tồn tại mối quan hệ cũ
            else if (existedArrangement != null && user.RoleId == 2)
            {
                if (arrangement.StartDate <= oldArrangementEnd.AddDays(-1))
                {
                    throw new BaseException(ErrorMessages.STARDATE_MUST_BIGGER_NOW);
                }
                var managerArrangement = arrangementRepo.GetManagerArrangementWithinDate(arrangement);
                if (managerArrangement != null)
                {
                    throw new BaseException(ErrorMessages.RELATIONSHIP_EXISTED);
                }
                var arrangementEntity = arrangement.ToEntity<Arrangement>();
                arrangementEntity.IsActive = true;
                unitOfWork.ArrangementRepository.Add(arrangementEntity);
                result = arrangementEntity.ToViewModel<ArrangementViewModel>();
            }
            else if (existedArrangement != null && user.RoleId == 2)
            {
                var managerArrangement = arrangementRepo.GetManagerArrangementWithinDate(arrangement);
                if (managerArrangement != null)
                {
                    throw new BaseException(ErrorMessages.RELATIONSHIP_EXISTED);
                }
                var arrangementEntity = arrangement.ToEntity<Arrangement>();
                arrangementEntity.IsActive = true;
                unitOfWork.ArrangementRepository.Add(arrangementEntity);
                result = arrangementEntity.ToViewModel<ArrangementViewModel>();
            }
            return result;
        }

        public ArrangementViewModel DeassignUserToLocation(ArrangementDisableViewModel arrangement)
        {
            var arrangementRepo = this.unitOfWork.ArrangementRepository;
            var arrangementDeassign = arrangementRepo.CheckLocationManagerWorker(arrangement.UserId, arrangement.LocationId, DateTime.Now);
            if (arrangementDeassign == null)
            {
                throw new BaseException(ErrorMessages.ARRANGEMENT_NOT_EXISTED);
            }
            arrangementDeassign.IsActive = false;
            arrangementRepo.Update(arrangementDeassign);
            return arrangementDeassign.ToViewModel<ArrangementViewModel>();
        }



        public void DeactiveOverdue()
        {
            var roomDeviceRepo = unitOfWork.RoomDeviceRepository;
            var arrangementRepo = unitOfWork.ArrangementRepository;

            using (var transaction = unitOfWork.CreateTransaction())
            {
                try
                {
                    var overdueRoomDevice = roomDeviceRepo.GetOverdue();
                    var overdueArrangement = arrangementRepo.GetOverdue();

                    foreach (var element in overdueRoomDevice)
                    {
                        element.IsActive = false;
                        roomDeviceRepo.Update(element);
                    }

                    foreach (var attribute in overdueArrangement)
                    {
                        attribute.IsActive = false;
                        arrangementRepo.Update(attribute);
                    }
                    transaction.Commit();
                }
                catch (Exception e)
                {

                    transaction.Rollback();
                    throw e;
                }

            }
        }
    }
}
