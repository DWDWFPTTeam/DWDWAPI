using System;
using System.Collections.Generic;

namespace DWDW_API.Core.Entities
{
    public partial class Record
    {
        public int? RecordId { get; set; }
        public int? DeviceId { get; set; }
        public int? Type { get; set; }
        public DateTime? RecordDateTime { get; set; }
        public string Image { get; set; }
        public virtual Device Device { get; set; }

    }
}
