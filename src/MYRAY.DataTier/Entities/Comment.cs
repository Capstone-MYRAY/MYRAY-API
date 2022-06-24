using System;
using System.Collections.Generic;

namespace MYRAY.DataTier.Entities
{
    public partial class Comment
    {
        public int Id { get; set; }
        public int GuidepostId { get; set; }
        public int CommentBy { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? Status { get; set; }

        public virtual Account CommentByNavigation { get; set; } = null!;
        public virtual Guidepost Guidepost { get; set; } = null!;
    }
}
