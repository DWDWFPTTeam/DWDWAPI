using DWDW_API.Core.Constants;
using DWDW_API.Core.Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DWDW_API.Core.ViewModels
{
    public class RecordViewModel : BaseModel
    {
        public int? RecordId { get; set; }
        public int? UserId { get; set; }
        public int? DeviceId { get; set; }
        public int? RoomId { get; set; }
        public string RoomCode { get; set; }
        public int? Type { get; set; }
        public DateTime? RecordDateTime { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
    }
    public class RecordRoomCodeViewModel : BaseModel
    {
        public int? RecordId { get; set; }
        public int? UserId { get; set; }
        public int? DeviceId { get; set; }
        public string RoomCode { get; set; }
        public int? Type { get; set; }
        public DateTime? RecordDateTime { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
    }

    public class RecordImageViewModel : BaseModel
    {
        public int? RecordId { get; set; }
        public int? UserId { get; set; }
        public int? DeviceId { get; set; }
        public string FullName { get; set; }
        public string RoomCode { get; set; }
        public int? Type { get; set; }
        public DateTime? RecordDateTime { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public byte[] ImageByte { get; set; }
    }
    public class RecordReceivedModel
    {
        [Required(ErrorMessage = ErrorMessages.DEVICE_CODE_IS_EMPTY)]
        public string DeviceCode { get; set; }
        [Required(ErrorMessage = ErrorMessages.TYPE_MUST_BE_INTEGER)]
        public int? Type { get; set; }

        [Required(ErrorMessage = ErrorMessages.IMAGE_IS_EMPTY)]
        public IFormFile Image { get; set; }

    }


    public class RecordReceivedByteModel
    {
        [Required(ErrorMessage = ErrorMessages.DEVICE_CODE_IS_EMPTY)]
        public string DeviceCode { get; set; }
        [Required(ErrorMessage = ErrorMessages.TYPE_MUST_BE_INTEGER)]
        public int? Type { get; set; }

        [Required(ErrorMessage = ErrorMessages.IMAGE_IS_EMPTY)]
        public byte[] Image { get; set; }

    }
    public class RecordLocationReceivedViewModel : BaseModel
    {
        [Required(ErrorMessage = ErrorMessages.LOCATIONID_INVALID)]
        public int LocationId { get; set; }

        [Required(ErrorMessage = ErrorMessages.START_DATE_REQUIRED)]
        [DataType(DataType.DateTime, ErrorMessage = ErrorMessages.WRONG_DATETIME_FORMAT)]
        [DisplayFormat(DataFormatString = "yyyy-MM-dd")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = ErrorMessages.END_DATE_REQUIRED)]
        [DataType(DataType.DateTime, ErrorMessage = ErrorMessages.WRONG_DATETIME_FORMAT)]
        [DisplayFormat(DataFormatString = "yyyy-MM-dd")]
        public DateTime EndDate { get; set; }
    }

    public class RecordStatusModel : BaseModel
    {
        [Required(ErrorMessage = ErrorMessages.RECORDID_IS_NOT_EXISTED)]
        public int? RecordId { get; set; }
        [Required(ErrorMessage = ErrorMessages.INVALID_DATE_FORMAT)]
        [DataType(DataType.DateTime, ErrorMessage = ErrorMessages.WRONG_DATETIME_FORMAT)]
        [DisplayFormat(DataFormatString = "yyyy-MM-dd")]
        public DateTime? RecordDateTime { get; set; }
        [Required(ErrorMessage = ErrorMessages.STATUS_INVALID)]
        public string Status { get; set; }
        [Required(ErrorMessage = ErrorMessages.COMMENT_INVALID)]
        public string Comment { get; set; }
    }
}
