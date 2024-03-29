﻿using System;
using System.Collections.Generic;

namespace MYRAY.DataTier.Entities
{
    public partial class JobPost
    {
        public JobPost()
        {
            AppliedJobs = new HashSet<AppliedJob>();
            ExtendTaskJobs = new HashSet<ExtendTaskJob>();
            Feedbacks = new HashSet<Feedback>();
            Messages = new HashSet<Message>();
            PaymentHistories = new HashSet<PaymentHistory>();
            PinDates = new HashSet<PinDate>();
            Reports = new HashSet<Report>();
            TreeJobs = new HashSet<TreeJob>();
        }

        public int Id { get; set; }
        public int GardenId { get; set; }
        public string Title { get; set; } = null!;
        public string Type { get; set; } = null!;
        public DateTime? StartJobDate { get; set; }
        public DateTime? EndJobDate { get; set; }
        public int? WorkTypeId { get; set; }
        public int? PublishedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int? Status { get; set; }
        public string? Description { get; set; }
        public int? StatusWork { get; set; }
        public string? ReasonReject { get; set; }
        public int? PostTypeId { get; set; }
        public int? TotalPinDay { get; set; }
        public DateTime? StartPinDate { get; set; }
        public DateTime? PublishedDate { get; set; }

        public virtual Account? ApprovedByNavigation { get; set; }
        public virtual Garden Garden { get; set; } = null!;
        public virtual PostType? PostType { get; set; }
        public virtual Account? PublishedByNavigation { get; set; }
        public virtual WorkType? WorkType { get; set; }
        public virtual PayPerHourJob PayPerHourJob { get; set; } = null!;
        public virtual PayPerTaskJob PayPerTaskJob { get; set; } = null!;
        public virtual ICollection<AppliedJob> AppliedJobs { get; set; }
        public virtual ICollection<ExtendTaskJob> ExtendTaskJobs { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<PaymentHistory> PaymentHistories { get; set; }
        public virtual ICollection<PinDate> PinDates { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<TreeJob> TreeJobs { get; set; }
    }
}
