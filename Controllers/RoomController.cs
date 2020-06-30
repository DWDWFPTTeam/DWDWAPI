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
    public class RoomController : BaseController
    {
        private readonly IRoomService roomService;
        public RoomController(ExtensionSettings extensionSettings, IRoomService roomService) : base(extensionSettings)
        {
            this.roomService = roomService;
            
        }
    }
}