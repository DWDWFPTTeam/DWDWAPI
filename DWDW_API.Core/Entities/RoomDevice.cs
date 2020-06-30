using System;
using System.Collections.Generic;

namespace DWDW_API.Core.Entities
{
    public partial class RoomDevice
    {
        public int? RoomDeviceId { get; set; }
        public int? RoomId { get; set; }
        public int? DeviceId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }

        public virtual Device Device { get; set; }
        public virtual Room Room { get; set; }
    }
}
