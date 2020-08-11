using System;
using System.Collections.Generic;

namespace DWDW_API.Core.Entities
{
    public partial class Location
    {
        public Location()
        {
            Arrangement = new HashSet<Arrangement>();
            Room = new HashSet<Room>();
        }

        public int? LocationId { get; set; }
        public string LocationCode { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Arrangement> Arrangement { get; set; }
        public virtual ICollection<Room> Room { get; set; }
    }
}
