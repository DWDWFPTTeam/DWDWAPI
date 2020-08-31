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
        [Required(ErrorMessage = ErrorMessages.ROOM_ID_INVALID)]
        public int? RoomId { get; set; }
        [Required(ErrorMessage = ErrorMessages.DEVICE_ID_INVALID)]
        public int? DeviceId { get; set; }
        [Required(ErrorMessage = ErrorMessages.DATE_INVALID)]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = ErrorMessages.DATE_INVALID)]
        public DateTime EndDate { get; set; }
    }

    public class RoomDeviceAssignModel : BaseModel
    {
        public int RoomDeviceId { get; set; }
        public int? RoomId { get; set; }
        public int? DeviceId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
        public string RoomCode { get; set; }
        public string DeviceCode { get; set; }
    }
}
