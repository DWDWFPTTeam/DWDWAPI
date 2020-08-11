using DWDW_API.Core.Infrastructure;
using DWDW_API.Core.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DWDW_API.Core.ViewModels
{
    public class LocationViewModel : BaseModel
    {
        [Required(ErrorMessage = ErrorMessages.LOCATIONID_INVALID)]
        public int LocationId { get; set; }
        [Required(ErrorMessage = ErrorMessages.LOCATION_CODE_INVALID)]
        public string LocationCode { get; set; }
        public bool? IsActive { get; set; }
    }
    public class LocationUserViewModel : BaseModel
    {
        public int LocationId { get; set; }
        public string LocationCode { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class LocationRecordViewModel : BaseModel
    {
        [Required(ErrorMessage = ErrorMessages.LOCATIONID_INVALID)]
        public int LocationId { get; set; }
        [Required(ErrorMessage = ErrorMessages.LOCATION_CODE_INVALID)]
        public string LocationCode { get; set; }
        public float TotalRecord { get; set; }
    }
    public class LocationUpdateModel : BaseModel
    {
        [Required(ErrorMessage = ErrorMessages.LOCATIONID_INVALID)]
        public int LocationId { get; set; }
        [Required(ErrorMessage = ErrorMessages.LOCATION_CODE_INVALID)]
        public string LocationCode { get; set; }
    }
    public class LocationUpdateStatusModel : BaseModel
    {
        [Required(ErrorMessage = ErrorMessages.LOCATIONID_INVALID)]
        public int LocationId { get; set; }
        [Required(ErrorMessage = ErrorMessages.STATUS_INVALID)]
        public bool? IsActive { get; set; }

    }
    public class LocationInsertModel : BaseModel
    {
        [Required(ErrorMessage = ErrorMessages.LOCATION_CODE_INVALID)]
        public string LocationCode { get; set; }
    }
    public class LocationReceiveDateModel : BaseModel
    {
        [Required(ErrorMessage = ErrorMessages.START_DATE_REQUIRED)]
        [DataType(DataType.DateTime, ErrorMessage = ErrorMessages.WRONG_DATETIME_FORMAT)]
        [DisplayFormat(DataFormatString = "yyyy-MM-dd")]
        public DateTime startDate { get; set; }

        [Required(ErrorMessage = ErrorMessages.END_DATE_REQUIRED)]
        [DataType(DataType.DateTime, ErrorMessage = ErrorMessages.WRONG_DATETIME_FORMAT)]
        [DisplayFormat(DataFormatString = "yyyy-MM-dd")]
        public DateTime endDate { get; set; }


    }

}
