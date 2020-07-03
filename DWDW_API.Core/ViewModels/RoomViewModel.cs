using DWDW_API.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DWDW_API.Core.ViewModels
{
    public class RoomViewModel : BaseModel
    {
        [Required]
        public int RoomId { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(10)]
        public string RoomCode { get; set; }
        [Required]
        public int LocationId { get; set; }
        public bool? IsActive { get; set; }
    }

    public class RoomInsertModel : BaseModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(10)]
        public string RoomCode { get; set; }
        [Required]
        public int LocationId { get; set; }
    }
    public class RoomUpdateModel : BaseModel
    {
        [Required]
        public int RoomId { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(10)]
        public string RoomCode { get; set; }
        [Required]
        public int LocationId { get; set; }
    }
}
