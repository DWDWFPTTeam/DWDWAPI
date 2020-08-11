using System;
using System.Collections.Generic;

namespace DWDW_API.Core.Entities
{
    public partial class Room
    {
        public Room()
        {
            RoomDevice = new HashSet<RoomDevice>();
            Shift = new HashSet<Shift>();
        }

        public int? RoomId { get; set; }
        public string RoomCode { get; set; }
        public int? LocationId { get; set; }
        public bool? IsActive { get; set; }

        public virtual Location Location { get; set; }
        public virtual ICollection<RoomDevice> RoomDevice { get; set; }
        public virtual ICollection<Shift> Shift { get; set; }
    }
}
