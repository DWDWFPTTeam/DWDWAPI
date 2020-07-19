using DWDW_API.Core.Constants;
using DWDW_API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_Service.Repositories
{
    public interface INotificationRepository : IBaseRepository<Notifications>
    {
        Notifications CreateNotification(Record record, Room room, User manager, User worker, string deviceCode);
    }
    public class NotificationRepository : BaseRepository<Notifications>, INotificationRepository
    {
        public NotificationRepository(DbContext dbContext) : base(dbContext)
        {

        }

        public Notifications CreateNotification(Record record, Room room, User manager, User worker, string deviceCode)
        {
            var notification = new Notifications()
            {
                MessageTime = record.RecordDateTime,
                Type = record.Type,
                UserId = manager.UserId,
                IsRead = false,
            };

            switch (notification.Type)
            {
                case Constant.ON:
                    notification.MessageTitle = "Device " + deviceCode + " at room " + room.RoomCode + " is on";
                    notification.MessageContent = "Worker + " + worker.UserName + " starts working at "
                        + record.RecordDateTime.ToString();
                    break;
                case Constant.OFF:
                    notification.MessageTitle = "Device " + deviceCode + " at room " + room.RoomCode + " is off";
                    notification.MessageContent = "Worker + " + worker.UserName + " stops working at "
                        + record.RecordDateTime.ToString();
                    break;
                case Constant.DROWSINESS:
                    notification.MessageTitle = "Device " + deviceCode + " at room " + room.RoomCode + " detects drowsiness";
                    notification.MessageContent = "Worker + " + worker.UserName + " is drowsy at "
                        + record.RecordDateTime.ToString();
                    break;
                case Constant.REST:
                    notification.MessageTitle = "Device " + deviceCode + " at room " + room.RoomCode + " temporarily rests";
                    notification.MessageContent = "Worker + " + worker.UserName + " turns off drowsiness detection mode for rest at "
                        + record.RecordDateTime.ToString();
                    break;
                case Constant.WORK:
                    notification.MessageTitle = "Device " + deviceCode + " at room " + room.RoomCode + " backs to work";
                    notification.MessageContent = "Worker + " + worker.UserName + " turns on drowsiness detection  at"
                        + record.RecordDateTime.ToString();
                    break;
                default:
                    break;
            }
            return notification;

        }
    }
}
