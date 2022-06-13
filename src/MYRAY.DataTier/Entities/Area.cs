using System;
using System.Collections.Generic;

namespace MYRAY.DataTier.Entities
{
    public partial class Area
    {
        public Area()
        {
            Gardens = new HashSet<Garden>();
            Accounts = new HashSet<Account>();
        }

        public int Id { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Commune { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<Garden> Gardens { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
