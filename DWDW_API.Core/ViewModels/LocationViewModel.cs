using DWDW_API.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DWDW_API.Core.ViewModels
{
    public class LocationViewModel : BaseModel
    {
        [Required]
        public int LocationId { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(10)]
        public string LocationCode { get; set; }
        public bool? IsActive { get; set; }
    }

    public class LocationRecordViewModel : BaseModel
    {
        [Required]
        public int LocationId { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(10)]
        public string LocationCode { get; set; }
        public float TotalRecord { get; set; }
    }
    public class LocationUpdateModel : BaseModel
    {
        [Required]
        public int LocationId { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(10)]
        public string LocationCode { get; set; }
    }
    public class LocationInsertModel : BaseModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(10)]
        public string LocationCode { get; set; }

    }
}
