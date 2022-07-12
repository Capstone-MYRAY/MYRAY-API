using System;
using System.Collections.Generic;

namespace MYRAY.DataTier.Entities
{
    public partial class Area
    {
        public Area()
        {
            AreaAccounts = new HashSet<AreaAccount>();
            Gardens = new HashSet<Garden>();
        }

        public int Id { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Commune { get; set; }
        public int? Status { get; set; }
        public string? Address { get; set; }

        public virtual ICollection<AreaAccount> AreaAccounts { get; set; }
        public virtual ICollection<Garden> Gardens { get; set; }
    }
}
