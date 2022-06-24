using System;
using System.Collections.Generic;

namespace MYRAY.DataTier.Entities
{
    public partial class Guidepost
    {
        public Guidepost()
        {
            Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Content { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? Status { get; set; }

        public virtual Account? CreatedByNavigation { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
