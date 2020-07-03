using DWDW_API.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DWDW_API.Core.ViewModels
{
    public class DeviceViewModel : BaseModel
    {
        public int DeviceId { get; set; }
        public string DeviceCode { get; set; }
        public bool? IsActive { get; set; }
    }

    public class DeviceCreateModel : BaseModel
    {
        [Required]
        public string DeviceCode { get; set; }
        [Required]
        public bool? IsActive { get; set; }
    }
    public class DeviceUpdateModel : BaseModel
    {
        [Required]
        public int DeviceId { get; set; }
        [Required]
        public string DeviceCode { get; set; }

    }
    public class DeviceActiveModel : BaseModel
    {
        [Required]
        public int DeviceId { get; set; }
        [Required]
        public bool? IsActive { get; set; }

    }
}
