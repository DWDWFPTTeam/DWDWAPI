using DWDW_API.Core.Constants;
using DWDW_API.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DWDW_API.Core.ViewModels
{
    public class ArrangementViewModel : BaseModel
    {
        public int? UserId { get; set; }
        public int? LocationId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }
    public class ArrangementReceivedViewModel : BaseModel
    {
        [Required(ErrorMessage = ErrorMessages.USERID_INVALID)]
        public int UserId { get; set; }
        [Required(ErrorMessage = ErrorMessages.LOCATIONID_INVALID)]
        public int LocationId { get; set; }
        [Required(ErrorMessage = ErrorMessages.DATE_INVALID)]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = ErrorMessages.DATE_INVALID)]
        public DateTime EndDate { get; set; }
    }

    public class ArrangementDisableViewModel : BaseModel
    {
        [Required(ErrorMessage = ErrorMessages.USERID_INVALID)]
        public int UserId { get; set; }
        [Required(ErrorMessage = ErrorMessages.LOCATIONID_INVALID)]
        public int LocationId { get; set; }
    }

    public class ArrangementLocationViewModel : BaseModel
    {
        public int? LocationId { get; set; }
        public string LocationCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public class ArrangementUserViewModel : BaseModel
    {
        public int UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
