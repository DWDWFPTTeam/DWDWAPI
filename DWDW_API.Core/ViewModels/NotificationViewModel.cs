using DWDW_API.Core.Entities;
using DWDW_API.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_API.Core.ViewModels
{
    public class NotificationViewModel: BaseModel
    {
        public int? UserId { get; set; }

        public string MessageTitle { get; set; }
        public DateTime? MessageTime { get; set; }
        public string MessageContent { get; set; }
        public int? Type { get; set; }
        public bool? IsRead { get; set; }

    }
    public class NotificationFCMViewModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string DeviceToken { get; set; }
        public NotificationViewModel NotificationVM { get;set; }
    }

}
