using System;
using System.Collections.Generic;

namespace DWDW_API.Core.Entities
{
    public partial class Device
    {
        public Device()
        {
            Record = new HashSet<Record>();
            RoomDevice = new HashSet<RoomDevice>();
        }

        public int? DeviceId { get; set; }
        public string DeviceCode { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Record> Record { get; set; }
        public virtual ICollection<RoomDevice> RoomDevice { get; set; }
    }
}
