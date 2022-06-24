using System;
using System.Collections.Generic;

namespace MYRAY.DataTier.Entities
{
    public partial class PostType
    {
        public PostType()
        {
            PinDates = new HashSet<PinDate>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public double Price { get; set; }
        public string? Color { get; set; }
        public int? Status { get; set; }
        public int? Background { get; set; }

        public virtual ICollection<PinDate> PinDates { get; set; }
    }
}
