using System;
using System.Collections.Generic;

namespace MYRAY.DataTier.Entities
{
    public partial class Report
    {
        public int Id { get; set; }
        public int JobPostId { get; set; }
        public string? Content { get; set; }
        public string? ResolveContent { get; set; }
        public int? ReportedId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public int? ResolvedBy { get; set; }
        public int? Status { get; set; }

        public virtual Account? CreatedByNavigation { get; set; }
        public virtual JobPost JobPost { get; set; } = null!;
        public virtual Account? ResolvedByNavigation { get; set; }
    }
}
