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
        public IActionResult GetAllNotifications()
        {
            IActionResult result;
            try
            {
                int userId = int.Parse(CurrentUserId);
                var notifications = notificationService.GetAllNotifiOfManager(userId);
                result = Ok(notifications);
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
        [Route("UpdateIsReadNotification")]
        [Authorize(Roles = Constant.MANAGER)]
        [HttpPut]
        public IActionResult UpdateIsReadNotification(int notificationId)
        {
            IActionResult result;
            try
            {
                var noti = this.notificationService.UpdateIsReadNotification(notificationId);
                result = Ok(noti);
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

    }
}
