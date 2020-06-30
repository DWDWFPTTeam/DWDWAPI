using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DWDW_API.Providers;
using DWDW_Service.Services;
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
    }
}