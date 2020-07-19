﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_API.Core.ViewModels
{
    class NotificationViewModel
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string MessageContent { get; set; }
        public int? Type { get; set; }
        public bool? IsRead { get; set; }


    }
}
