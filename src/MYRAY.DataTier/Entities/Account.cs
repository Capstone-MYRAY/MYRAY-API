using System;
using System.Collections.Generic;

namespace MYRAY.DataTier.Entities
{
    public partial class Account
    {
        public Account()
        {
            AppliedJobs = new HashSet<AppliedJob>();
            Attendances = new HashSet<Attendance>();
            Comments = new HashSet<Comment>();
            FeedbackBelongeds = new HashSet<Feedback>();
            FeedbackCreatedByNavigations = new HashSet<Feedback>();
            Gardens = new HashSet<Garden>();
            Guideposts = new HashSet<Guidepost>();
            JobPostApprovedByNavigations = new HashSet<JobPost>();
            JobPostPublishedByNavigations = new HashSet<JobPost>();
            MessageFroms = new HashSet<Message>();
            MessageTos = new HashSet<Message>();
            PaymentHistoryBelongeds = new HashSet<PaymentHistory>();
            PaymentHistoryCreatedByNavigations = new HashSet<PaymentHistory>();
            ReportCreatedByNavigations = new HashSet<Report>();
            ReportResolvedByNavigations = new HashSet<Report>();
            Accounts = new HashSet<Account>();
            Areas = new HashSet<Area>();
            Bookmarks = new HashSet<Account>();
        }

        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Fullname { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set; }
        public string? Address { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string? Email { get; set; }
        public string Password { get; set; } = null!;
        public double Balance { get; set; }
        public int Point { get; set; }
        public string? AboutMe { get; set; }
        public bool? Status { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<AppliedJob> AppliedJobs { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Feedback> FeedbackBelongeds { get; set; }
        public virtual ICollection<Feedback> FeedbackCreatedByNavigations { get; set; }
        public virtual ICollection<Garden> Gardens { get; set; }
        public virtual ICollection<Guidepost> Guideposts { get; set; }
        public virtual ICollection<JobPost> JobPostApprovedByNavigations { get; set; }
        public virtual ICollection<JobPost> JobPostPublishedByNavigations { get; set; }
        public virtual ICollection<Message> MessageFroms { get; set; }
        public virtual ICollection<Message> MessageTos { get; set; }
        public virtual ICollection<PaymentHistory> PaymentHistoryBelongeds { get; set; }
        public virtual ICollection<PaymentHistory> PaymentHistoryCreatedByNavigations { get; set; }
        public virtual ICollection<Report> ReportCreatedByNavigations { get; set; }
        public virtual ICollection<Report> ReportResolvedByNavigations { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Area> Areas { get; set; }
        public virtual ICollection<Account> Bookmarks { get; set; }
    }
}
