using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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


        //This API just for testing 
        [Route("GetAllAllowAnonymous")]
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAllUserAllowAnonymous()
        {
            IActionResult result;
            try
            {
                var users = userService.GetAllAllowAnonymous();
                result = Ok(users);
            }
            catch (BaseException e)
            {
                result = BadRequest(e.Message);
            }
            catch (Exception e)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            return result;
        }

        [Route("GetUserInfoToken")]
        [Authorize]
        [HttpGet]
        public IActionResult GetUserInfoToken()
        {
            IActionResult result;
            try
            {
                var userId = int.Parse(CurrentUserId);
                var user = userService.GetUserById(userId);
                result = Ok(user);
            }
            catch (BaseException e)
            {
                result = BadRequest(e.Message);
            }
            catch (Exception e)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            return result;
        }


        [Route("GetAll")]
        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        public IActionResult GetAllUserByAdmin()
        {
            IActionResult result;
            try
            {
                var users = userService.GetAllByAdmin();
                result = Ok(users);
            }
            catch (BaseException e)
            {
                result = BadRequest(e.Message);
            }
            catch (Exception e)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            return result;
        }


        [Route("GetUserFromLocationByAdmin")]
        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        public IActionResult GetUserFromLocationByAdmin(int locationId)
        {
            IActionResult result;
            try
            {
                var users = userService.GetUserFromLocationByAdmin(locationId);
                return Ok(users);
            }
            catch (BaseException e)
            {
                result = BadRequest(e.Message);
            }
            catch (Exception e)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            return result;
        }

        [Route("GetUserFromLocationsByManager")]
        [Authorize(Roles = Constant.MANAGER)]
        [HttpGet]
        public IActionResult GetUserFromLocationsByManager()
        {
            IActionResult result;
            try
            {
                int userId = int.Parse(CurrentUserId);
                var users = userService.GetUserFromLocationsByManager(userId);
                result = Ok(users);
            }
            catch (BaseException e)
            {
                result = BadRequest(e.Message);
            }
            catch (Exception e)
            {

                result = StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            return result;
        }

        [Route("GetUserFromOneLocationByManager")]
        [Authorize(Roles = Constant.MANAGER)]
        [HttpGet]
        public IActionResult GetUserFromOneLocationByManager(int locationId)
        {
            IActionResult result;
            try
            {
                int userId = int.Parse(CurrentUserId);
                var users = userService.GetUserFromOneLocationByManager(userId, locationId);
                result = Ok(users);
            }
            catch (BaseException e)
            {
                result = BadRequest(e.Message);
            }
            catch (Exception e)
            {

                result = StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            return result;
        }




        [Route("CraeteUserByAdmin")]
        [Authorize(Roles = Constant.ADMIN)]
        [HttpPost]
        public IActionResult CraeteUserByAdmin(UserCreateModel userCreated)
        {
            IActionResult result;
            try
            {
                var insert = userService.CreateUserAsync(userCreated);
                result = Ok(insert);
            }
            catch (BaseException e)
            {
                result = BadRequest(e.Message);
            }
            catch (Exception e)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            return result;
        }

        [Route("LoginAsync")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(UserLoginInfo info)
        {
            IActionResult result;
            try
            {

                var user = await userService.LoginAsync(info.UserName, info.Password);
                if (user != null)
                {
                    var tokenModel = new TokenResponseModel();
                    tokenModel.AccessToken = jwtTokenProvider.CreateUserAccessToken(user);
                    result = Ok(tokenModel);
                }
                else
                {
                    result = BadRequest(ErrorMessages.INVALID_USERNAME_PASSWORD);
                }
            } //handle exception
            catch (BaseException e)
            {
                result = BadRequest(e.Message);
            }
            catch (Exception e)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            return result;
        }


        [Route("UpdateUserByAdmin")]
        [Authorize(Roles = Constant.ADMIN)]
        [HttpPut]
        public IActionResult UpdateUserByAdmin(UserUpdateModel userUpdate)
        {
            IActionResult result;
            try
            {
                var userUpdated = userService.UpdateUser(userUpdate);
                return Ok(userUpdated);
            }
            catch (BaseException e)
            {
                result = BadRequest(e.Message);
            }
            catch (Exception e)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            return result;
        }

        [Route("DeActiveUserByAdmin")]
        [Authorize(Roles = Constant.ADMIN)]
        [HttpDelete]
        public IActionResult DeActiveUserByAdmin(int id)
        {
            IActionResult result;
            try
            {
                var deActiveUser = userService.DeActiveUserByAdmin(id);
                result = Ok(deActiveUser);
            }
            catch (BaseException e)
            {
                result = BadRequest(e.Message);
            }
            catch (Exception e)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            return result;
        }

        [Route("UpdateManagerDeviceToken")]
        [Authorize(Roles = Constant.MANAGER)]
        [HttpDelete]
        public IActionResult UpdateManagerDeviceToken(string deviceToken)
        {
            IActionResult result;
            var identity = (ClaimsIdentity)User.Identity;
            var ID = (identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            int userID = int.Parse(ID);
            try
            {
                var managerDeviceToken = userService.UpdateManagerDeviceToken(userID ,deviceToken);
                result = Ok(managerDeviceToken);
            }
            catch(BaseException e)
            {
                result = BadRequest(e.Message);
            }
            catch (Exception e)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            return result;
        }

        }

}