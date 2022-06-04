using System;
using System.Collections.Generic;

namespace MYRAY.DataTier.Entities
{
    public partial class Garden
    {
        public Garden()
        {
            JobPosts = new HashSet<JobPost>();
        }

        public int Id { get; set; }
        public int AreaId { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; } = null!;
        public decimal? Latitudes { get; set; }
        public decimal? Longitudes { get; set; }
        public double? LandArea { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime CreateDate { get; set; }
        public int Status { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual Area Area { get; set; } = null!;
        public virtual ICollection<JobPost> JobPosts { get; set; }
    }
}
