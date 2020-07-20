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
        public DateTime? Date { get; set; }
        public int? RoomId { get; set; }
        public bool? IsActive { get; set; }
    }
    public class ShiftCreateModel : BaseModel
    {
        [Required]
        public int? ArrangementId { get; set; }
        [DataType(DataType.DateTime, ErrorMessage = ErrorMessages.WRONG_DATETIME_FORMAT)]
        public DateTime? Date { get; set; }
        [Required]
        public int? RoomId { get; set; }
    }
    public class ShiftUpdateModel : BaseModel
    {
        [Required]
        public int ShiftId { get; set; }
        [Required]
        public int? ArrangementId { get; set; }
        [DataType(DataType.DateTime, ErrorMessage = ErrorMessages.WRONG_DATETIME_FORMAT)]
        public DateTime? Date { get; set; }
        [Required]
        public int? RoomId { get; set; }
    }
    public class ShiftActiveModel : BaseModel
    {
        [Required]
        public int ShiftId { get; set; }
        
        [Required]
        public bool? IsActive { get; set; }
    }
}
