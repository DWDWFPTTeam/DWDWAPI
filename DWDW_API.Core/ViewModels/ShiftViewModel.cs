using DWDW_API.Core.Constants;
using DWDW_API.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DWDW_API.Core.ViewModels
{
    public class ShiftViewModel : BaseModel
    {
        public int ShiftId { get; set; }
        public int? ArrangementId { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public DateTime? Date { get; set; }
        public int? RoomId { get; set; }
        public string RoomCode { get; set; }
        public bool? IsActive { get; set; }
    }
    public class ShiftCreateModel : BaseModel
    {
        [Required(ErrorMessage = ErrorMessages.USERID_INVALID)]
        public int WorkerID { get; set; }
        [DataType(DataType.DateTime, ErrorMessage = ErrorMessages.WRONG_DATETIME_FORMAT)]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = ErrorMessages.ROOM_ID_INVALID)]
        public int RoomId { get; set; }
        [Required(ErrorMessage = ErrorMessages.LOCATIONID_INVALID)]
        public int LocationId { get; set;}
    }
    public class ShiftUpdateModel : BaseModel
    {
        [Required(ErrorMessage = ErrorMessages.SHIFT_ID_INVALID)]
        public int ShiftId { get; set; }
        [Required(ErrorMessage = ErrorMessages.USERID_INVALID)]
        public int WorkerID { get; set; }
        [DataType(DataType.DateTime, ErrorMessage = ErrorMessages.WRONG_DATETIME_FORMAT)]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = ErrorMessages.ROOM_ID_INVALID)]
        public int? RoomId { get; set; }
    }
    public class ShiftActiveModel : BaseModel
    {
        [Required(ErrorMessage = ErrorMessages.SHIFT_ID_INVALID)]
        public int ShiftId { get; set; }
        
        [Required(ErrorMessage = ErrorMessages.STATUS_INVALID)]
        public bool? IsActive { get; set; }
    }
}
