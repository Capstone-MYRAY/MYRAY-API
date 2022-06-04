using System;
using System.Collections.Generic;

namespace MYRAY.DataTier.Entities
{
    public partial class Role
    {
        public Role()
        {
            Accounts = new HashSet<Account>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int Status { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
