using System;
using DWDW_API.Core.Constants;
using DWDW_API.Core.Infrastructure;
using DWDW_API.Core.ViewModels;
using DWDW_API.Providers;
using DWDW_Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DWDW_API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserService userService;

        private readonly JwtTokenProvider jwtTokenProvider;

        public UserController(ExtensionSettings extensionSettings
                            , IUserService userService, JwtTokenProvider jwtTokenProvider) : base(extensionSettings)
        {
            this.userService = userService;
            this.jwtTokenProvider = jwtTokenProvider;
        }


        [Route("GetAllAllowAnonymous")]
        [AllowAnonymous]
        [HttpGet]
        public dynamic GetAllUserAllowAnonymous()
        {
            return ExecuteInMonitoring(() =>
            {
                return userService.GetAllAllowAnonymous();
            });
        }

        //[Route("GetUserInfoToken")]
        //[Authorize]
        //[HttpGet]
        //public IActionResult GetUserInfoToken()
        //{
        //    IActionResult result;
        //    try
        //    {
        //        var userId = int.Parse(CurrentUserId);
        //        var user = userService.GetUserById(userId);
        //        result = Ok(user);
        //    }
        //    catch (BaseException e)
        //    {
        //        result = BadRequest(new ErrorViewModel
        //        {
        //            StatusCode = StatusCodes.Status400BadRequest,
        //            Message = e.Message
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        result = StatusCode(StatusCodes.Status500InternalServerError, new ErrorViewModel
        //        {
        //            StatusCode = StatusCodes.Status500InternalServerError,
        //            Message = e.Message
        //        });
        //    }
        //    return result;
        //}
        [Route("GetUserInfoToken")]
        [Authorize]
        [HttpGet]
        public dynamic GetUserInfoToken()
        {
            return ExecuteInMonitoring(() =>
            {
                var userId = int.Parse(CurrentUserId);
                return userService.GetUserById(userId);
            });
        }

        [Route("GetAllByAdmin")]
        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        public dynamic GetAllUserByAdmin()
        {
            return ExecuteInMonitoring(() =>
            {
                return userService.GetAllByAdmin();
            });
        }

        [Route("GetByIDAdmin")]
        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        public dynamic GetByIDAdmin(int userId)
        {
            return ExecuteInMonitoring(() =>
            {
                return userService.GetByIDAdmin(userId);
            });
        }

        [Route("GetAllActive")]
        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        public dynamic GetAllActiveUserByAdmin()
        {
            return ExecuteInMonitoring(() =>
            {
                var userId = int.Parse(CurrentUserId);
                return userService.GetAllActiveByAdmin(userId);
            });
        }

        //[Route("GetAllActive")]
        //[Authorize(Roles = Constant.ADMIN)]
        //[HttpGet]
        //public IActionResult GetAllActiveUserByAdmin()
        //{
        //    IActionResult result;
        //    try
        //    {
        //        var users = userService.GetAllActiveByAdmin();
        //        result = Ok(users);
        //    }
        //    catch (BaseException e)
        //    {
        //        result = BadRequest(new ErrorViewModel
        //        {
        //            StatusCode = StatusCodes.Status400BadRequest,
        //            Message = e.Message
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        result = StatusCode(StatusCodes.Status500InternalServerError, new ErrorViewModel
        //        {
        //            StatusCode = StatusCodes.Status500InternalServerError,
        //            Message = e.Message
        //        });
        //    }
        //    return result;
        //}


        [Route("GetUserFromLocationByAdmin")]
        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        public IActionResult GetUserFromLocationByAdmin(int locationId)
        {
            return ExecuteInMonitoring(() =>
            {
                
                return userService.GetUserFromLocationByAdmin( locationId);
            });
        }

        [Route("GetUserFromLocationsByManager")]
        [Authorize(Roles = Constant.MANAGER)]
        [HttpGet]
        public IActionResult GetUserFromLocationsByManager()
        {
            return ExecuteInMonitoring(() =>
            {
                int userId = int.Parse(CurrentUserId);
                return userService.GetUserFromLocationsByManager(userId);
            });
        }

        [Route("GetUserFromOneLocationByManager")]
        [Authorize(Roles = Constant.MANAGER)]
        [HttpGet]
        public IActionResult GetUserFromOneLocationByManager(int locationId)
        {
            return ExecuteInMonitoring(() =>
            {
                int userId = int.Parse(CurrentUserId);
                return userService.GetUserFromOneLocationByManager(userId, locationId);
            });
        }

        [Route("GetWorkerFromLocationsByManager")]
        [Authorize(Roles = Constant.MANAGER)]
        [HttpGet]
        public IActionResult GetWorkerFromLocationsByManager(int locationID)
        {
            return ExecuteInMonitoring(() =>
            {
                int userID = int.Parse(CurrentUserId);
                return userService.GetWorkerFromLocationsByManager(userID, locationID);
            });
        }



        //chi fix Craete -> Create
        [Route("CreateUserByAdmin")]
        [Authorize(Roles = Constant.ADMIN)]
        [HttpPost]
        public IActionResult CreateUserByAdmin(UserCreateModel userCreated)
        {
            return ExecuteInMonitoring(() =>
            {
                return userService.CreateUser(userCreated);
            });
        }

        [Route("Login")]
        [AllowAnonymous]
        [HttpPost]
        public dynamic LoginAsync(UserLoginInfo info)
        {
            return ExecuteInMonitoring(() =>
            {
                var user = userService.LoginAsync(info.UserName, info.Password);
                userService.UpdateUserDeviceToken(user.UserId.Value, info.DeviceToken);
                var tokenModel = new TokenResponseModel();
                return jwtTokenProvider.CreateUserAccessToken(user);
            });
        }

        [Route("UpdateUserByAdmin")]
        [Authorize(Roles = Constant.ADMIN)]
        [HttpPut]
        public dynamic UpdateUserByAdmin(UserUpdateModel userUpdate)
        {
            return ExecuteInMonitoring(() =>
            {
                return userService.UpdateUser(userUpdate);
            });
        }

        [Route("AssignUserToLocationByAdmin")]
        [Authorize(Roles = Constant.ADMIN)]
        [HttpPut]
        public dynamic AssignUserToLocation(ArrangementReceivedViewModel arrangement)
        {
            return ExecuteInMonitoring(() =>
            {
                return userService.AssignUserToLocation(arrangement);
            });
        }
       

        [Route("DeassignUserFromLocationByAdmin")]
        [Authorize(Roles = Constant.ADMIN)]
        [HttpPut]
        public dynamic DeassignUserFromLocationByAdmin(int arrangementID)
        {
            return ExecuteInMonitoring(() =>
            {
                return userService.DeassignUserToLocation(arrangementID);
            });
        }

        [Route("UpdatePersonalInfo")]
        [Authorize(Roles = Constant.ADMIN + ", " + Constant.MANAGER + ", " + Constant.WORKER)]
        [HttpPut]
        public dynamic UpdatePersonalInfo(UserPersonalUpdateModel userUpdate)
        {
            return ExecuteInMonitoring(() =>
            {
                var userId = int.Parse(CurrentUserId);
                return userService.UpdatePersonalInfo(userId, userUpdate);
            });
        }


        [Route("UpdateManagerDeviceToken")]
        [Authorize(Roles = Constant.MANAGER)]
        [HttpPut]
        public dynamic UpdateManagerDeviceToken(string deviceToken)
        {
            return ExecuteInMonitoring(() =>
            {
                var userId = int.Parse(CurrentUserId);
                return userService.UpdateUserDeviceToken(userId, deviceToken);
            });
        }



        [Route("DeActiveUserByAdmin")]
        [Authorize(Roles = Constant.ADMIN)]
        [HttpDelete]
        public dynamic DeActiveUserByAdmin(int id)
        {
            return ExecuteInMonitoring(() =>
            {
                return userService.DeActiveUserByAdmin(id);
            });
        }
        [Route("UpdateUserActiveByAdmin")]
        [Authorize(Roles = Constant.ADMIN)]
        [HttpPut]
        public IActionResult ActiveUserByAdmin(UserActiveModel user)
        {
            IActionResult result;
            try
            {
                var activeUser = userService.ActiveUserByAdmin(user);
                result = Ok(activeUser);
            }
            catch (BaseException e)
            {
                result = BadRequest(new ErrorViewModel
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = e.Message
                });
            }
            catch (Exception e)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new ErrorViewModel
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = e.Message
                });
            }
            return result;
        }



        //[Route("UpdateManagerDeviceToken")]
        //[Authorize(Roles = Constant.MANAGER)]
        //[HttpDelete]
        //public IActionResult UpdateManagerDeviceToken(string deviceToken)
        //{
        //    IActionResult result;
        //    var identity = (ClaimsIdentity)User.Identity;
        //    var ID = (identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        //    int userID = int.Parse(ID);
        //    try
        //    {
        //        var managerDeviceToken = userService.UpdateUserDeviceToken(userID, deviceToken);
        //        result = Ok(managerDeviceToken);
        //    }
        //    catch (BaseException e)
        //    {
        //        result = BadRequest(new ErrorViewModel
        //        {
        //            StatusCode = StatusCodes.Status400BadRequest,
        //            Message = e.Message
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        result = StatusCode(StatusCodes.Status500InternalServerError, new ErrorViewModel
        //        {
        //            StatusCode = StatusCodes.Status500InternalServerError,
        //            Message = e.Message
        //        });
        //    }
        //    return result;
        //}




    }

}