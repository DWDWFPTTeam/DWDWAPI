using DWDW_API.Core.Constants;
using DWDW_API.Core.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace DWDW_API.Core.ViewModels
{
    public class RoomViewModel : BaseModel
    {
        [Required(ErrorMessage = ErrorMessages.ROOM_ID_INVALID)]
        public int RoomId { get; set; }
        [Required(ErrorMessage = ErrorMessages.ROOM_CODE_INVALID)]
        public string RoomCode { get; set; }
        [Required(ErrorMessage = ErrorMessages.LOCATIONID_INVALID)]
        public int LocationId { get; set; }
        public bool? IsActive { get; set; }
    }

    public class RoomInsertModel : BaseModel
    {
        [Required(ErrorMessage = ErrorMessages.ROOM_CODE_INVALID)]
        public string RoomCode { get; set; }
        [Required(ErrorMessage = ErrorMessages.LOCATIONID_INVALID)]
        public int LocationId { get; set; }
    }
    public class RoomUpdateModel : BaseModel
    {
        [Required(ErrorMessage = ErrorMessages.ROOM_ID_INVALID)]
        public int RoomId { get; set; }
        [Required(ErrorMessage = ErrorMessages.ROOM_CODE_INVALID)]
        public string RoomCode { get; set; }
        [Required(ErrorMessage = ErrorMessages.LOCATIONID_INVALID)]
        public int LocationId { get; set; }
    }
}
