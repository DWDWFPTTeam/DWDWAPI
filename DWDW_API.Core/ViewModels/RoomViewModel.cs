using DWDW_API.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_API.Core.ViewModels
{
    public class RoomViewModel : BaseModel
    {
        public int RoomId { get; set; }
        public string RoomCode { get; set; }
        public int? LocationId { get; set; }
        public bool? IsActive { get; set; }
    }
}
