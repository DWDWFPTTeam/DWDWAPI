using System;
using System.Collections.Generic;

namespace DWDW_API.Core.Entities
{
    public partial class User
    {
        public User()
        {
            Arrangement = new HashSet<Arrangement>();
        }

        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Gender { get; set; }
        public string DeviceToken { get; set; }
        public int? RoleId { get; set; }
        public bool? IsActive { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Arrangement> Arrangement { get; set; }
    }
}
