using System;
using System.Collections.Generic;
using System.Linq;
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


        //unfinished
        [Route("GetWorkerFromLocationByAdmin")]
        [Authorize(Roles = Constant.ADMIN)]
        [HttpGet]
        public IActionResult GetWorkerFromLocationByAdmin(int locationId)
        {
            IActionResult result;
            try
            {
                //unfinished
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




        [Route("CraeteUserByAdmin")]
        [Authorize(Roles = Constant.ADMIN)]
        [HttpPost]
        public IActionResult CraeteUser(UserCreateModel userCreated)
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

                    string accessToken = jwtTokenProvider.CreateAccesstoken(user);

                    result = Ok(accessToken);
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
 
    }

}