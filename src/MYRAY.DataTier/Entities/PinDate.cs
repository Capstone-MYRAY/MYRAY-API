using System;
using System.Collections.Generic;

namespace MYRAY.DataTier.Entities
{
    public partial class PinDate
    {
        public int Id { get; set; }
        public int PostTypeId { get; set; }
        public int JobPostId { get; set; }
        public DateTime PinDate1 { get; set; }
        public int? Status { get; set; }

        public virtual JobPost JobPost { get; set; } = null!;
        public virtual PostType PostType { get; set; } = null!;
    }
}
