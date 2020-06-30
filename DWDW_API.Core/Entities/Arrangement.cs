using System;
using System.Collections.Generic;

namespace DWDW_API.Core.Entities
{
    public partial class Arrangement
    {
        public Arrangement()
        {
            Shift = new HashSet<Shift>();
        }

        public int? ArrangementId { get; set; }
        public int? UserId { get; set; }
        public int? LocationId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }

        public virtual Location Location { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Shift> Shift { get; set; }
    }
}
