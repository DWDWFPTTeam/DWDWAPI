using DWDW_API.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_API.Core.ViewModels
{
    public class DeviceViewModel : BaseModel
    {
        public int DeviceId { get; set; }
        public string DeviceCode { get; set; }
        public int? DeviceStatus { get; set; }
        public bool? IsActive { get; set; }
    }
}
