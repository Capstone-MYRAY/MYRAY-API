using System;
using System.Collections.Generic;

namespace MYRAY.DataTier.Entities
{
    public partial class TreeType
    {
        public TreeType()
        {
            TreeJobs = new HashSet<TreeJob>();
        }

        public int Id { get; set; }
        public string Type { get; set; } = null!;
        public string? Description { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<TreeJob> TreeJobs { get; set; }
    }
}
