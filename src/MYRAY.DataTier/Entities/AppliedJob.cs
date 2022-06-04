using System;
using System.Collections.Generic;

namespace MYRAY.DataTier.Entities
{
    public partial class AppliedJob
    {
        public AppliedJob()
        {
            Attendances = new HashSet<Attendance>();
        }

        public int Id { get; set; }
        public int AppliedBy { get; set; }
        public int JobPostId { get; set; }
        public DateTime AppliedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Status { get; set; }

        public virtual Account AppliedByNavigation { get; set; } = null!;
        public virtual JobPost JobPost { get; set; } = null!;
        public virtual ICollection<Attendance> Attendances { get; set; }
    }
}
