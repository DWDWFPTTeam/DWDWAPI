using System;
using System.Collections.Generic;

namespace DWDW_API.Core.Entities
{
    public partial class Shift
    {
        public int? ShiftId { get; set; }
        public int? ArrangementId { get; set; }
        public DateTime? Date { get; set; }
        public int? RoomId { get; set; }
        public int? ShiftType { get; set; }
        public bool? IsActive { get; set; }

        public virtual Arrangement Arrangement { get; set; }
        public virtual Room Room { get; set; }
    }
}
