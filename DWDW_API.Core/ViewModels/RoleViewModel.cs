using DWDW_API.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DWDW_API.Core.ViewModels
{
    public class RoleViewModel : BaseModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool? IsActive { get; set; }
    }

    public class RoleCreateModel : BaseModel
    {
        [Required]
        public string RoleName { get; set; }
    }
    public class RoleActiveModel : BaseModel
    {
        [Required]
        public int RoleId { get; set; }
        [Required]

        public bool? IsActive { get; set; }
    }
}
