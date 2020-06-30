using DWDW_API.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_API.Core.ViewModels
{
    public class RecordViewModel : BaseModel
    {
        public int RecordId { get; set; }
        public int? DeviceId { get; set; }
        public DateTime? RecordDate { get; set; }
        public string Image { get; set; }
        public int? RecordStatus { get; set; }
        public bool? IsActive { get; set; }
    }
}
