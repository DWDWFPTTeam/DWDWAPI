using DWDW_API.Core.Constants;
using DWDW_API.Core.Entities;
using DWDW_API.Core.Infrastructure;
using DWDW_API.Core.ViewModels;
using DWDW_Service.Repositories;
using DWDW_Service.UnitOfWorks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DWDW_Service.Services
{
    public interface INotificationService : IBaseService<Notifications>
    {
        IEnumerable<NotificationViewModel> GetAllNotifiOfManager(int userId);
        NotificationViewModel UpdateIsReadNotification(int notificationId);
    }
    public class NotificationService : BaseService<Notifications>, INotificationService
    {
        private readonly INotificationRepository notificationRepository;
        public NotificationService(UnitOfWork unitOfWork, INotificationRepository notificationRepository) : base(unitOfWork)
        {
            this.notificationRepository = notificationRepository;
        }

        public IEnumerable<NotificationViewModel> GetAllNotifiOfManager(int userId)
        {
            IEnumerable<NotificationViewModel> result;
            var user = unitOfWork.UserRepository.Find(userId);
            if (user == null)
            {
                throw new BaseException(ErrorMessages.USERID_IS_NOT_EXISTED);
            }
            if (user.RoleId != int.Parse(Constant.MANAGER))
            {
                throw new BaseException(ErrorMessages.ROLE_IS_NOT_MANAGER);
            }
            result = this.notificationRepository.Get(n => n.UserId == userId, null, "")
                         .Select(n => n.ToViewModel<NotificationViewModel>());

            return result;
        }

        public NotificationViewModel UpdateIsReadNotification(int notificationId)
        {
            NotificationViewModel result;
            var noti = notificationRepository.Find(notificationId);
            if (noti == null)
            {
                throw new BaseException(ErrorMessages.NOTI_ID_IS_NOT_EXISTED);
            }
            noti.IsRead = true;
            notificationRepository.Update(noti);
            result = noti.ToViewModel<NotificationViewModel>();
            return result;
        }
    }
}
