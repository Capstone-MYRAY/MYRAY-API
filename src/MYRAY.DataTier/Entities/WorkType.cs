using System;
using System.Collections.Generic;

namespace MYRAY.DataTier.Entities
{
    public partial class WorkType
    {
        public WorkType()
        {
            JobPosts = new HashSet<JobPost>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<JobPost> JobPosts { get; set; }
    }
}
