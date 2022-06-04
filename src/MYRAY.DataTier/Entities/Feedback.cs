using System;
using System.Collections.Generic;

namespace MYRAY.DataTier.Entities
{
    public partial class Feedback
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public byte NumStar { get; set; }
        public int JobPostId { get; set; }
        public int BelongedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedBy { get; set; }

        public virtual Account Belonged { get; set; } = null!;
        public virtual Account CreatedByNavigation { get; set; } = null!;
        public virtual JobPost JobPost { get; set; } = null!;
    }
}
