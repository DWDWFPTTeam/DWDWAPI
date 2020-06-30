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
                var users = userService.GetAll();
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




        [Route("CreateUserAsync")]
        [Authorize]
        [HttpPost]
        public  IActionResult CraeteUserAsync([FromQuery]UserCreateModel userCreated)
        {
            IActionResult result;
            try
            {
                var insert =  userService.CreateUserAsync(userCreated);
                result = Ok(insert);
            }
            catch (BaseException e)
            {
                result = BadRequest(e.Message);
            }
            catch (Exception e)
            {
                result =  StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            return result;
        }

        [Route("LoginAsync")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromQuery]UserLoginInfo info)
        {
            IActionResult result;
            try
            {
                
                var user = await userService.LoginAsync(info.UserName, info.Password);
                if(user != null)
                {

                    string accessToken = jwtTokenProvider.CreateAccesstoken(user);

                    result = Ok(accessToken);
                }
                else
                {
                    result = BadRequest(ErrorMessages.INVALID_USERNAME_PASSWORD);
                }
            } //handle exception
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