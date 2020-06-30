using DWDW_API.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_API.Core.ViewModels
{
    public class ShiftViewModel : BaseModel
    {
        public int ShiftId { get; set; }
        public int? ArrangementId { get; set; }
        public DateTime? Date { get; set; }
        public int? RoomId { get; set; }
        public int? ShiftType { get; set; }
        public bool? IsActive { get; set; }
    }
}
