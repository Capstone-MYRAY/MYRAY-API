using System;
using System.Collections.Generic;

namespace MYRAY.DataTier.Entities
{
    public partial class Attendance
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public double? Salary { get; set; }
        public int? BonusPoint { get; set; }
        public string? Signature { get; set; }
        public int? Status { get; set; }
        public int AppliedJobId { get; set; }
        public int AccountId { get; set; }
        public string? Reason { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual AppliedJob AppliedJob { get; set; } = null!;
    }
}
