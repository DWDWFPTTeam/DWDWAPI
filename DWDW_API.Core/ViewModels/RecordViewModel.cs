using DWDW_API.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DWDW_API.Core.ViewModels
{
    public class RecordViewModel : BaseModel
    {
        public int? RecordId { get; set; }
        public int? DeviceId { get; set; }
        public int? Type { get; set; }
        public DateTime? RecordDateTime { get; set; }
        public string Image { get; set; }
    }
    public class RecordReceivedModel
    {
        [Required]
        public string DeviceCode { get; set; }
        [Required]
        public int? Type { get; set; }

        [Required]
        public string Image { get; set; }
    }
}
