using DWDW_API.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_API.Core.ViewModels
{
    public class RoomDeviceViewModel : BaseModel
    {
        public int RoomDeviceId { get; set; }
        public int? RoomId { get; set; }
        public int? DeviceId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
