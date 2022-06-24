using System;
using System.Collections.Generic;

namespace MYRAY.DataTier.Entities
{
    public partial class Bookmark
    {
        public int AccountId { get; set; }
        public int BookmarkId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual Account BookmarkNavigation { get; set; } = null!;
    }
}
