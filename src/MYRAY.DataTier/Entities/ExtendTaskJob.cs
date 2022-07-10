using System;
using System.Collections.Generic;

namespace MYRAY.DataTier.Entities
{
    public partial class ExtendTaskJob
    {
        public int Id { get; set; }
        public int? JobPostId { get; set; }
        public int? RequestBy { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? ExtendEndDate { get; set; }
        public string? Reason { get; set; }
        public int? Status { get; set; }
        public DateTime? OldEndDate { get; set; }

        public virtual Account? ApprovedByNavigation { get; set; }
        public virtual JobPost? JobPost { get; set; }
        public virtual Account? RequestByNavigation { get; set; }
    }
}
