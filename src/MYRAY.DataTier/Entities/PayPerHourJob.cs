using System;
using System.Collections.Generic;

namespace MYRAY.DataTier.Entities
{
    public partial class PayPerHourJob
    {
        public int Id { get; set; }
        public int EstimatedTotalTask { get; set; }
        public int MinFarmer { get; set; }
        public int MaxFarmer { get; set; }
        public double Salary { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? FinishTime { get; set; }

        public virtual JobPost IdNavigation { get; set; } = null!;
    }
}
