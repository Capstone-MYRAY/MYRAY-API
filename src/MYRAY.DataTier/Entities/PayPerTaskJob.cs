using System;
using System.Collections.Generic;

namespace MYRAY.DataTier.Entities
{
    public partial class PayPerTaskJob
    {
        public int Id { get; set; }
        public DateTime? FinishTime { get; set; }
        public double? Salary { get; set; }
        public bool? IsFarmToolsAvaiable { get; set; }

        public virtual JobPost IdNavigation { get; set; } = null!;
    }
}
