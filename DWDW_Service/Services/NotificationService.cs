using DWDW_API.Core.Entities;
using DWDW_Service.UnitOfWorks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_Service.Services
{
    public interface INotificationService : IBaseService<Notifications>
    {

    }
    public class NotificationService : BaseService<Notifications>, INotificationService
    {
        public NotificationService(UnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
      
    }
}
