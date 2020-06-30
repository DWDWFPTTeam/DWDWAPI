using DWDW_API.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_API.Core.ViewModels
{
    public class LocationViewModel : BaseModel
    {
        public int LocationId { get; set; }
        public string LocationCode { get; set; }
        public bool? IsActive { get; set; }
    }
}
