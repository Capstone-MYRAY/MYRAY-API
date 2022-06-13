using System;
using System.Collections.Generic;

namespace MYRAY.DataTier.Entities
{
    public partial class TreeType
    {
        public TreeType()
        {
            JobPosts = new HashSet<JobPost>();
        }

        public int Id { get; set; }
        public string Type { get; set; } = null!;
        public string? Description { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<JobPost> JobPosts { get; set; }
    }
}
