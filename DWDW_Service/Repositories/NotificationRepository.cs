using DWDW_API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_Service.Repositories
{
    public interface INotificationRepository : IBaseRepository<Notifications>
    {

    }
    public class NotificationRepository : BaseRepository<Notifications>, INotificationRepository
    {
        public NotificationRepository(DbContext dbContext) : base(dbContext)
        {

        }

        

    }
}
