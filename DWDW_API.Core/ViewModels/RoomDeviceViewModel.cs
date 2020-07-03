using DWDW_API.Core.Constants;
using DWDW_API.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DWDW_API.Core.ViewModels
{
    public class RoomDeviceViewModel : BaseModel
    {
        public int RoomDeviceId { get; set; }
        public int? RoomId { get; set; }
        public int? DeviceId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }
    public class RoomDeviceCreateModel : BaseModel
    {
        [Required]
        public int? RoomId { get; set; }
        [Required]
        public int? DeviceId { get; set; }
        [DataType(DataType.DateTime, ErrorMessage = ErrorMessages.BIRTHDAY_WRONG_DATETIME_FORMAT)]
        public DateTime? StartDate { get; set; }
        [DataType(DataType.DateTime, ErrorMessage = ErrorMessages.BIRTHDAY_WRONG_DATETIME_FORMAT)]
        public DateTime? EndDate { get; set; }
    }
}
