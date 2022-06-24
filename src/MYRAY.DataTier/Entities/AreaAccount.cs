using System;
using System.Collections.Generic;

namespace MYRAY.DataTier.Entities
{
    public partial class AreaAccount
    {
        public int AccountId { get; set; }
        public int AreaId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual Area Area { get; set; } = null!;
    }
}
