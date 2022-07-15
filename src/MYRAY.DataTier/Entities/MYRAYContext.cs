using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MYRAY.DataTier.Entities
{
    public partial class MYRAYContext : DbContext
    {
        public MYRAYContext()
        {
        }

        public MYRAYContext(DbContextOptions<MYRAYContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<AppliedJob> AppliedJobs { get; set; } = null!;
        public virtual DbSet<Area> Areas { get; set; } = null!;
        public virtual DbSet<AreaAccount> AreaAccounts { get; set; } = null!;
        public virtual DbSet<Attendance> Attendances { get; set; } = null!;
        public virtual DbSet<Bookmark> Bookmarks { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<ExtendTaskJob> ExtendTaskJobs { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<Garden> Gardens { get; set; } = null!;
        public virtual DbSet<Guidepost> Guideposts { get; set; } = null!;
        public virtual DbSet<JobPost> JobPosts { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<PayPerHourJob> PayPerHourJobs { get; set; } = null!;
        public virtual DbSet<PayPerTaskJob> PayPerTaskJobs { get; set; } = null!;
        public virtual DbSet<PaymentHistory> PaymentHistories { get; set; } = null!;
        public virtual DbSet<PinDate> PinDates { get; set; } = null!;
        public virtual DbSet<PostType> PostTypes { get; set; } = null!;
        public virtual DbSet<Report> Reports { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<TreeJob> TreeJobs { get; set; } = null!;
        public virtual DbSet<TreeType> TreeTypes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.HasIndex(e => e.PhoneNumber, "Uni__Phone")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AboutMe).HasColumnName("about_me");

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .HasColumnName("address");

                entity.Property(e => e.Balance)
                    .HasColumnName("balance")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("date")
                    .HasColumnName("date_of_birth");

                entity.Property(e => e.Email)
                    .HasMaxLength(320)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Fullname)
                    .HasMaxLength(50)
                    .HasColumnName("fullname");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.ImageUrl).HasColumnName("image_url");

                entity.Property(e => e.Password)
                    .HasMaxLength(256)
                    .HasColumnName("password");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("phone_number");

                entity.Property(e => e.Point)
                    .HasColumnName("point")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.RefreshToken)
                    .HasMaxLength(500)
                    .HasColumnName("refresh_token");

                entity.Property(e => e.RefreshTokenExpiryTime)
                    .HasColumnType("datetime")
                    .HasColumnName("refresh_token_expiry_time");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_Role");
            });

            modelBuilder.Entity<AppliedJob>(entity =>
            {
                entity.ToTable("AppliedJob");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AppliedBy).HasColumnName("applied_by");

                entity.Property(e => e.AppliedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("applied_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ApprovedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("approved_date");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("end_date");

                entity.Property(e => e.JobPostId).HasColumnName("job_post_id");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("start_date");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.AppliedByNavigation)
                    .WithMany(p => p.AppliedJobs)
                    .HasForeignKey(d => d.AppliedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AppliedJob_Account");

                entity.HasOne(d => d.JobPost)
                    .WithMany(p => p.AppliedJobs)
                    .HasForeignKey(d => d.JobPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AppliedJob_JobPost");
            });

            modelBuilder.Entity<Area>(entity =>
            {
                entity.ToTable("Area");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.Commune)
                    .HasMaxLength(100)
                    .HasColumnName("commune");

                entity.Property(e => e.District)
                    .HasMaxLength(100)
                    .HasColumnName("district");

                entity.Property(e => e.Province)
                    .HasMaxLength(100)
                    .HasColumnName("province");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<AreaAccount>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.AreaId });

                entity.ToTable("AreaAccount");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.AreaId).HasColumnName("area_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AreaAccounts)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AreaAccount_Account");

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.AreaAccounts)
                    .HasForeignKey(d => d.AreaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AreaAccount_Area");
            });

            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.ToTable("Attendance");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.AppliedJobId).HasColumnName("applied_job_id");

                entity.Property(e => e.BonusPoint)
                    .HasColumnName("bonus_point")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Reason).HasColumnName("reason");

                entity.Property(e => e.Salary).HasColumnName("salary");

                entity.Property(e => e.Signature).HasColumnName("signature");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Attendances)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Attendance_Account");

                entity.HasOne(d => d.AppliedJob)
                    .WithMany(p => p.Attendances)
                    .HasForeignKey(d => d.AppliedJobId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Attendance_AppliedJob");
            });

            modelBuilder.Entity<Bookmark>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.BookmarkId });

                entity.ToTable("Bookmark");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.BookmarkId).HasColumnName("bookmark_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.BookmarkAccounts)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Bookmark_Account");

                entity.HasOne(d => d.BookmarkNavigation)
                    .WithMany(p => p.BookmarkBookmarkNavigations)
                    .HasForeignKey(d => d.BookmarkId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Bookmark_Account1");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CommentBy).HasColumnName("comment_by");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.GuidepostId).HasColumnName("guidepost_id");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_date");

                entity.HasOne(d => d.CommentByNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.CommentBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_Account");

                entity.HasOne(d => d.Guidepost)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.GuidepostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_Guidepost");
            });

            modelBuilder.Entity<ExtendTaskJob>(entity =>
            {
                entity.ToTable("ExtendTaskJob");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApprovedBy).HasColumnName("approved_by");

                entity.Property(e => e.ApprovedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("approved_date");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.ExtendEndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("extend_end_date");

                entity.Property(e => e.JobPostId).HasColumnName("job_post_id");

                entity.Property(e => e.OldEndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("old_end_date");

                entity.Property(e => e.Reason).HasColumnName("reason");

                entity.Property(e => e.RequestBy).HasColumnName("request_by");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.ApprovedByNavigation)
                    .WithMany(p => p.ExtendTaskJobApprovedByNavigations)
                    .HasForeignKey(d => d.ApprovedBy)
                    .HasConstraintName("FK_ExtendTaskJob_Account1");

                entity.HasOne(d => d.JobPost)
                    .WithMany(p => p.ExtendTaskJobs)
                    .HasForeignKey(d => d.JobPostId)
                    .HasConstraintName("FK_ExtendTaskJob_JobPost");

                entity.HasOne(d => d.RequestByNavigation)
                    .WithMany(p => p.ExtendTaskJobRequestByNavigations)
                    .HasForeignKey(d => d.RequestBy)
                    .HasConstraintName("FK_ExtendTaskJob_Account");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BelongedId).HasColumnName("belonged_id");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.JobPostId).HasColumnName("job_post_id");

                entity.Property(e => e.NumStar).HasColumnName("num_star");

                entity.HasOne(d => d.Belonged)
                    .WithMany(p => p.FeedbackBelongeds)
                    .HasForeignKey(d => d.BelongedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Feedback_Account1");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.FeedbackCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Feedback_Account");

                entity.HasOne(d => d.JobPost)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.JobPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Feedback_JobPost");
            });

            modelBuilder.Entity<Garden>(entity =>
            {
                entity.ToTable("Garden");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.AreaId).HasColumnName("area_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.ImageUrl).HasColumnName("image_url");

                entity.Property(e => e.LandArea).HasColumnName("land_area");

                entity.Property(e => e.Latitudes)
                    .HasColumnType("decimal(8, 6)")
                    .HasColumnName("latitudes");

                entity.Property(e => e.Longitudes)
                    .HasColumnType("decimal(9, 6)")
                    .HasColumnName("longitudes");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_date");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Gardens)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Garden_Account");

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.Gardens)
                    .HasForeignKey(d => d.AreaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Garden_Area");
            });

            modelBuilder.Entity<Guidepost>(entity =>
            {
                entity.ToTable("Guidepost");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_date");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Guideposts)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Guidepost_Account");
            });

            modelBuilder.Entity<JobPost>(entity =>
            {
                entity.ToTable("JobPost");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApprovedBy).HasColumnName("approved_by");

                entity.Property(e => e.ApprovedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("approved_date");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.EndJobDate)
                    .HasColumnType("datetime")
                    .HasColumnName("end_job_date");

                entity.Property(e => e.GardenId).HasColumnName("garden_id");

                entity.Property(e => e.NumPublishDay).HasColumnName("num_publish_day");

                entity.Property(e => e.PostTypeId).HasColumnName("post_type_id");

                entity.Property(e => e.PublishedBy).HasColumnName("published_by");

                entity.Property(e => e.PublishedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("published_date");

                entity.Property(e => e.ReasonReject).HasColumnName("reason_reject");

                entity.Property(e => e.StartJobDate)
                    .HasColumnType("date")
                    .HasColumnName("start_job_date");

                entity.Property(e => e.StartPinDate)
                    .HasColumnType("datetime")
                    .HasColumnName("start_pin_date");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.StatusWork)
                    .HasColumnName("status_work")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Title)
                    .HasMaxLength(70)
                    .HasColumnName("title");

                entity.Property(e => e.TotalPinDay)
                    .HasColumnName("total_pin_day")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("type");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_date");

                entity.HasOne(d => d.ApprovedByNavigation)
                    .WithMany(p => p.JobPostApprovedByNavigations)
                    .HasForeignKey(d => d.ApprovedBy)
                    .HasConstraintName("FK_JobPost_Account");

                entity.HasOne(d => d.Garden)
                    .WithMany(p => p.JobPosts)
                    .HasForeignKey(d => d.GardenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_JobPost_Garden");

                entity.HasOne(d => d.PostType)
                    .WithMany(p => p.JobPosts)
                    .HasForeignKey(d => d.PostTypeId)
                    .HasConstraintName("FK_JobPost_PostType");

                entity.HasOne(d => d.PublishedByNavigation)
                    .WithMany(p => p.JobPostPublishedByNavigations)
                    .HasForeignKey(d => d.PublishedBy)
                    .HasConstraintName("FK_JobPost_Account1");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Message");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.ConventionId)
                    .IsUnicode(false)
                    .HasColumnName("convention_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FromId).HasColumnName("from_id");

                entity.Property(e => e.ImageUrl).HasColumnName("image_url");

                entity.Property(e => e.IsRead)
                    .HasColumnName("is_read")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.JobPostId).HasColumnName("job_post_id");

                entity.Property(e => e.ToId).HasColumnName("to_id");

                entity.HasOne(d => d.From)
                    .WithMany(p => p.MessageFroms)
                    .HasForeignKey(d => d.FromId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Message_Account");

                entity.HasOne(d => d.JobPost)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.JobPostId)
                    .HasConstraintName("FK_Message_JobPost");

                entity.HasOne(d => d.To)
                    .WithMany(p => p.MessageTos)
                    .HasForeignKey(d => d.ToId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Message_Account1");
            });

            modelBuilder.Entity<PayPerHourJob>(entity =>
            {
                entity.ToTable("PayPerHourJob");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.EstimatedTotalTask).HasColumnName("estimated_total_task");

                entity.Property(e => e.FinishTime)
                    .HasColumnType("time(0)")
                    .HasColumnName("finish_time");

                entity.Property(e => e.MaxFarmer).HasColumnName("max_farmer");

                entity.Property(e => e.MinFarmer).HasColumnName("min_farmer");

                entity.Property(e => e.Salary).HasColumnName("salary");

                entity.Property(e => e.StartTime)
                    .HasColumnType("time(0)")
                    .HasColumnName("start_time");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.PayPerHourJob)
                    .HasForeignKey<PayPerHourJob>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PayPerHourJob_JobPost");
            });

            modelBuilder.Entity<PayPerTaskJob>(entity =>
            {
                entity.ToTable("PayPerTaskJob");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.FinishTime)
                    .HasColumnType("datetime")
                    .HasColumnName("finish_time");

                entity.Property(e => e.IsFarmToolsAvaiable)
                    .HasColumnName("is_farm_tools_avaiable")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Salary).HasColumnName("salary");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.PayPerTaskJob)
                    .HasForeignKey<PayPerTaskJob>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PayPerTaskJob_JobPost1");
            });

            modelBuilder.Entity<PaymentHistory>(entity =>
            {
                entity.ToTable("PaymentHistory");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActualPrice).HasColumnName("actual_price");

                entity.Property(e => e.Balance).HasColumnName("balance");

                entity.Property(e => e.BalanceFluctuation).HasColumnName("balance_fluctuation");

                entity.Property(e => e.BelongedId).HasColumnName("belonged_id");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EarnedPoint).HasColumnName("earned_point");

                entity.Property(e => e.JobPostId).HasColumnName("job_post_id");

                entity.Property(e => e.JobPostPrice).HasColumnName("job_post_price");

                entity.Property(e => e.Message)
                    .HasMaxLength(100)
                    .HasColumnName("message");

                entity.Property(e => e.NumberPublishedDay).HasColumnName("number_published_day");

                entity.Property(e => e.PointPrice).HasColumnName("point_price");

                entity.Property(e => e.PostTypePrice).HasColumnName("post_type_price");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TotalPinDay)
                    .HasColumnName("total_pin_day")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.UsedPoint).HasColumnName("used_point");

                entity.HasOne(d => d.Belonged)
                    .WithMany(p => p.PaymentHistoryBelongeds)
                    .HasForeignKey(d => d.BelongedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PaymentHistory_Account");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.PaymentHistoryCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_PaymentHistory_Account1");

                entity.HasOne(d => d.JobPost)
                    .WithMany(p => p.PaymentHistories)
                    .HasForeignKey(d => d.JobPostId)
                    .HasConstraintName("FK_PaymentHistory_JobPost");
            });

            modelBuilder.Entity<PinDate>(entity =>
            {
                entity.ToTable("PinDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.JobPostId).HasColumnName("job_post_id");

                entity.Property(e => e.PinDate1)
                    .HasColumnType("datetime")
                    .HasColumnName("pin_date");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.JobPost)
                    .WithMany(p => p.PinDates)
                    .HasForeignKey(d => d.JobPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PinDate_JobPost");
            });

            modelBuilder.Entity<PostType>(entity =>
            {
                entity.ToTable("PostType");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Background)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("background");

                entity.Property(e => e.Color)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("color");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("Report");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.JobPostId).HasColumnName("job_post_id");

                entity.Property(e => e.ReportedId).HasColumnName("reported_id");

                entity.Property(e => e.ResolveContent).HasColumnName("resolve_content");

                entity.Property(e => e.ResolvedBy).HasColumnName("resolved_by");

                entity.Property(e => e.ResolvedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("resolved_date");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ReportCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Report_Account");

                entity.HasOne(d => d.JobPost)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.JobPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Report_JobPost");

                entity.HasOne(d => d.Reported)
                    .WithMany(p => p.ReportReporteds)
                    .HasForeignKey(d => d.ReportedId)
                    .HasConstraintName("FK_Report_Account2");

                entity.HasOne(d => d.ResolvedByNavigation)
                    .WithMany(p => p.ReportResolvedByNavigations)
                    .HasForeignKey(d => d.ResolvedBy)
                    .HasConstraintName("FK_Report_Account1");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<TreeJob>(entity =>
            {
                entity.HasKey(e => new { e.TreeTypeId, e.JobPostId });

                entity.ToTable("TreeJob");

                entity.Property(e => e.TreeTypeId).HasColumnName("tree_type_id");

                entity.Property(e => e.JobPostId).HasColumnName("job_post_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.HasOne(d => d.JobPost)
                    .WithMany(p => p.TreeJobs)
                    .HasForeignKey(d => d.JobPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TreeJob_JobPost");

                entity.HasOne(d => d.TreeType)
                    .WithMany(p => p.TreeJobs)
                    .HasForeignKey(d => d.TreeTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TreeJob_TreeType");
            });

            modelBuilder.Entity<TreeType>(entity =>
            {
                entity.ToTable("TreeType");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("type");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
