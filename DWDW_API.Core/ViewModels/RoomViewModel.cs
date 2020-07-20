using DWDW_API.Core.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace DWDW_API.Core.ViewModels
{
    public class RoomViewModel : BaseModel
    {
        [Required]
        public int RoomId { get; set; }
        [Required]
        public string RoomCode { get; set; }
        [Required]
        public int LocationId { get; set; }
        public bool? IsActive { get; set; }
    }

    public class RoomInsertModel : BaseModel
    {
        [Required]
        public string RoomCode { get; set; }
        [Required]
        public int LocationId { get; set; }
    }
    public class RoomUpdateModel : BaseModel
    {
        [Required]
        public int RoomId { get; set; }
        [Required]
        public string RoomCode { get; set; }
        [Required]
        public int LocationId { get; set; }
    }
}
