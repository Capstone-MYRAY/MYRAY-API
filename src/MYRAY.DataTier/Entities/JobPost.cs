using System;
using System.Collections.Generic;

namespace MYRAY.DataTier.Entities
{
    public partial class JobPost
    {
        public JobPost()
        {
            AppliedJobs = new HashSet<AppliedJob>();
            Feedbacks = new HashSet<Feedback>();
            Messages = new HashSet<Message>();
            PaymentHistories = new HashSet<PaymentHistory>();
            PinDates = new HashSet<PinDate>();
            Reports = new HashSet<Report>();
        }

        public int Id { get; set; }
        public int GardenId { get; set; }
        public int TreeTypeId { get; set; }
        public string Title { get; set; } = null!;
        public string Type { get; set; } = null!;
        public DateTime? StartJobDate { get; set; }
        public DateTime? EndJobDate { get; set; }
        public int? NumPublishDay { get; set; }
        public int? PublishedBy { get; set; }
        public DateTime? PublishedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int? Status { get; set; }
        public string? Description { get; set; }
        public int? StatusWork { get; set; }

        public virtual Account? ApprovedByNavigation { get; set; }
        public virtual Garden Garden { get; set; } = null!;
        public virtual Account? PublishedByNavigation { get; set; }
        public virtual TreeType TreeType { get; set; } = null!;
        public virtual PayPerHourJob PayPerHourJob { get; set; } = null!;
        public virtual PayPerTaskJob PayPerTaskJob { get; set; } = null!;
        public virtual ICollection<AppliedJob> AppliedJobs { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<PaymentHistory> PaymentHistories { get; set; }
        public virtual ICollection<PinDate> PinDates { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
