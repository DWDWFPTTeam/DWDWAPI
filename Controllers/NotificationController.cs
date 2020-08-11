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
    public class NotificationController : BaseController
    {
        private readonly INotificationService notificationService;
        public NotificationController(ExtensionSettings extensionSettings, INotificationService notificationService) : base(extensionSettings)
        {
            this.notificationService = notificationService;
        }
        [Route("GetAllNotifications")]
        [Authorize(Roles = Constant.MANAGER)]
        [HttpGet]
        public dynamic GetAllNotifications()
        {
            int userId = int.Parse(CurrentUserId);
            return ExecuteInMonitoring(() =>
            {
                return notificationService.GetAllNotifiOfManager(userId);
            });
        }
        [Route("UpdateIsReadNotification")]
        [Authorize(Roles = Constant.MANAGER)]
        [HttpPut]
        public dynamic UpdateIsReadNotification(int notificationId)
        {
            return ExecuteInMonitoring(() =>
            {
                return notificationService.UpdateIsReadNotification(notificationId);
            });
        }

    }
}
