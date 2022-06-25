using System;
using System.Collections.Generic;

namespace MYRAY.DataTier.Entities
{
    public partial class TreeJob
    {
        public int TreeTypeId { get; set; }
        public int JobPostId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual JobPost JobPost { get; set; } = null!;
        public virtual TreeType TreeType { get; set; } = null!;
    }
}
