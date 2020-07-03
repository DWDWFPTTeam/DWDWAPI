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
    public class RoleController : BaseController
    {
        private readonly IRoleService roleService;
        public RoleController(ExtensionSettings extensionSettings, IRoleService roleService) : base(extensionSettings)
        {
            this.roleService = roleService;
        }

        [Route("GetAll")]
        [HttpGet]
        public IActionResult GetRoles()
        {
            IActionResult result;
            try
            {
                var roles = roleService.GetAll();
                result = Ok(roles);
            } catch (BaseException e)
            {
                result = BadRequest(e.Message);
            }
            catch (Exception e)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, e.Message);

            }
            return result;
        }

        [Authorize(Roles = Constant.ADMIN)]
        [Route("CreateRole")]
        [HttpPost]
        public IActionResult CreateRoles([FromQuery] RoleCreateModel roleCreated)
        {
            IActionResult result;
            try
            {
                var insert = roleService.CreateRole(roleCreated);
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

        [Authorize(Roles = Constant.ADMIN)]
        [Route("UpdateRoleActive")]
        [HttpPut]
        public IActionResult UpdateRoleActive([FromQuery] RoleActiveModel roleActive)
        {
            IActionResult result;
            try
            {
                var update = roleService.UpdateRoleActive(roleActive);
                result = Ok(update);
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