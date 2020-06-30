using System;
using System.Collections.Generic;

namespace DWDW_API.Core.Entities
{
    public partial class Role
    {
        public Role()
        {
            User = new HashSet<User>();
        }

        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}
