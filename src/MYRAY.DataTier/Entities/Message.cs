using System;
using System.Collections.Generic;

namespace MYRAY.DataTier.Entities
{
    public partial class Message
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public int FromId { get; set; }
        public int ToId { get; set; }
        public int? JobPostId { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ConventionId { get; set; }

        public virtual Account From { get; set; } = null!;
        public virtual JobPost? JobPost { get; set; }
        public virtual Account To { get; set; } = null!;
    }
}
