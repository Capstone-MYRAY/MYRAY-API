using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MYRAY.Business.DTOs.JobPost;
using MYRAY.Business.Enums;
using MYRAY.Business.Exceptions;
using MYRAY.Business.Repositories.Interface;
using MYRAY.Business.Services.Notification;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.JobPost;

public class JobPostRepository : IJobPostRepository
{
    private readonly IDbContextFactory _contextFactory;
    private readonly IBaseRepository<DataTier.Entities.JobPost> _jobPostRepository;
    private readonly IBaseRepository<PayPerHourJob> _payPerHourRepository;
    private readonly IBaseRepository<PayPerTaskJob> _payPerTaskRepository;
    private readonly IBaseRepository<PinDate> _pinDateRepository;
    private readonly IBaseRepository<DataTier.Entities.PaymentHistory> _paymentHistoryRepository;
    private readonly IBaseRepository<DataTier.Entities.Account> _accountRepository;
    private readonly IBaseRepository<DataTier.Entities.TreeJob> _treeJobRepository;
    private readonly IBaseRepository<DataTier.Entities.AppliedJob> _appliedRepository;
    private readonly IBaseRepository<DataTier.Entities.Attendance> _attendanceRepository;

    public JobPostRepository(IDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
        var context = _contextFactory.GetContext<MYRAYContext>();
        _jobPostRepository = context.GetRepository<DataTier.Entities.JobPost>()!;
        _payPerHourRepository = context.GetRepository<PayPerHourJob>()!;
        _payPerTaskRepository = context.GetRepository<PayPerTaskJob>()!;
        _pinDateRepository = context.GetRepository<PinDate>()!;
        _paymentHistoryRepository = context.GetRepository<DataTier.Entities.PaymentHistory>()!;
        _accountRepository = context.GetRepository<DataTier.Entities.Account>()!;
        _treeJobRepository = context.GetRepository<DataTier.Entities.TreeJob>()!;
        _appliedRepository = context.GetRepository<DataTier.Entities.AppliedJob>()!;
        _attendanceRepository = context.GetRepository<DataTier.Entities.Attendance>()!;
    }

    public IQueryable<DataTier.Entities.JobPost> GetJobPosts(int? publishBy = null)
    {
        Expression<Func<DataTier.Entities.JobPost, object>> expHours = post => post.PayPerHourJob;
        Expression<Func<DataTier.Entities.JobPost, object>> expTask = post => post.PayPerTaskJob;
        Expression<Func<DataTier.Entities.JobPost, object>> expGarden = post => post.Garden;
        Expression<Func<DataTier.Entities.JobPost, object>> expArea = post => post.Garden.Area;
        Expression<Func<DataTier.Entities.JobPost, object>> expWork = post => post.WorkType;
        Expression<Func<DataTier.Entities.JobPost, object>> expTreeJob = post => post.TreeJobs;
        bool flag = publishBy != null;
        IQueryable<DataTier.Entities.JobPost> query = _jobPostRepository.Get(post =>
                post.Status != (int?)JobPostEnum.JobPostStatus.Deleted &&
                (!flag || post.PublishedBy == publishBy),
            new[] { expHours, expTask, expGarden, expArea, expWork, expTreeJob });
        return query;
    }

    public IQueryable<DataTier.Entities.JobPost> GetPinPost()
    {
        Expression<Func<DataTier.Entities.JobPost, object>> expHours = post => post.PayPerHourJob;
        Expression<Func<DataTier.Entities.JobPost, object>> expTask = post => post.PayPerTaskJob;
        Expression<Func<DataTier.Entities.JobPost, object>> expGarden = post => post.Garden;
        Expression<Func<DataTier.Entities.JobPost, object>> expArea = post => post.Garden.Area;
        Expression<Func<DataTier.Entities.JobPost, object>> expWork = post => post.WorkType;
        Expression<Func<DataTier.Entities.JobPost, object>> expTreeJob = post => post.TreeJobs;
        IQueryable<PinDate> pinDates =
            _pinDateRepository.Get(p => p.PinDate1.Date.Equals(DateTime.Today) && p.Status == 1);
        var postId = pinDates.Select(p => p.JobPostId);
        IQueryable<DataTier.Entities.JobPost> jobPosts = _jobPostRepository.Get(null, 
            new[] { expHours, expTask , expGarden, expArea, expWork, expTreeJob});
        jobPosts = jobPosts.Where(p => postId.Contains(p.Id));
        return jobPosts;
    }

    public IQueryable<PinDate> GetExistedPinDateOnRange(DateTime publishedDate, int numberPublishDay, int postTypeId)
    {
        DateTime endPublishedDate = publishedDate.Date.AddDays(numberPublishDay);
        IQueryable<PinDate> pinDates = _pinDateRepository.Get(pd => pd.PinDate1 >= publishedDate
                                                                    && pd.PinDate1 <= endPublishedDate
                                                                    && pd.JobPost.PostTypeId == postTypeId
                                                                    && pd.Status == 1);
        return pinDates;
    }

    public IQueryable<PinDate> GetNearPinDateByPinDate(DateTime pinDate, int postTypeId)
    {
        IQueryable<PinDate> nearPinDate =
            _pinDateRepository.Get(pd => pd.PinDate1.Date > pinDate.Date
                                         && pd.JobPost.PostTypeId == postTypeId
                                         && pd.Status == 1)
                .OrderBy(pd => pd.PinDate1);

        return nearPinDate;
    }

    public IQueryable<DataTier.Entities.JobPost> GetJobAvailableByGardenId(int gardenId)
    {
        IQueryable<DataTier.Entities.JobPost> query = _jobPostRepository.Get(jobPost => jobPost.GardenId == gardenId
            && (jobPost.StatusWork == 1
                || jobPost.Status == (int)JobPostEnum.JobPostStatus.Pending
                || jobPost.Status == (int)JobPostEnum.JobPostStatus.Posted)
        );

        return query;
    }

    public IQueryable<DataTier.Entities.JobPost> GetInProgressJobPost()
    {
        Expression<Func<DataTier.Entities.JobPost, object>> expAppliedJob = post => post.AppliedJobs;
        IQueryable<DataTier.Entities.JobPost> inProgress = _jobPostRepository.Get(
            post => post.StatusWork == (int?)JobPostEnum.JobPostWorkStatus.Started, new[] { expAppliedJob });
        return inProgress;
    }

    public async Task<DataTier.Entities.JobPost> GetJobPostById(int id)
    {
        Expression<Func<DataTier.Entities.JobPost, object>> expHours = post => post.PayPerHourJob;
        Expression<Func<DataTier.Entities.JobPost, object>> expTask = post => post.PayPerTaskJob;
        Expression<Func<DataTier.Entities.JobPost, object>> expGarden = post => post.Garden;
        Expression<Func<DataTier.Entities.JobPost, object>> expTree = post => post.TreeJobs;
        Expression<Func<DataTier.Entities.JobPost, object>> expWorkType = post => post.WorkType;

        DataTier.Entities.JobPost jobPost = (await _jobPostRepository.GetFirstOrDefaultAsync(
            j => j.Id == id && j.Status != (int?)JobPostEnum.JobPostStatus.Deleted,
            new[] { expHours, expTask, expGarden, expTree, expWorkType }))!;
        return jobPost;
    }

    public async Task<PayPerHourJob?> GetPayPerHourJob(int jobPostId)
    {
        return await _payPerHourRepository.GetFirstOrDefaultAsync(pph => pph.Id == jobPostId);
    }

    public async Task<PayPerTaskJob?> GetPayPerTaskJob(int jobPostId)
    {
        return await _payPerTaskRepository.GetFirstOrDefaultAsync(pph => pph.Id == jobPostId);
    }

    public async Task<DataTier.Entities.JobPost> CreateJobPost(
        DataTier.Entities.JobPost jobPost,
        PayPerHourJob? payPerHourJob,
        PayPerTaskJob? payPerTaskJob,
        ICollection<PinDate>? pinDates,
        DataTier.Entities.PaymentHistory newPayment)
    {
        if (pinDates != null)
        {
            jobPost.PinDates = pinDates;
        }

        jobPost.PaymentHistories = new List<DataTier.Entities.PaymentHistory> { newPayment };
        await _jobPostRepository.InsertAsync(jobPost);
        if (jobPost.Type.Equals("PayPerHourJob"))
        {
            if (payPerHourJob == null) throw new Exception("PayPerHourJob: Empty");

            await _payPerHourRepository.InsertAsync(payPerHourJob);
        }
        else
        {
            if (payPerTaskJob == null) throw new Exception("PayPerTaskJob: Empty");
            await _payPerTaskRepository.InsertAsync(payPerTaskJob);
        }

        if (pinDates != null)
        {
            foreach (var variable in pinDates)
            {
                variable.JobPostId = jobPost.Id;
                await _pinDateRepository.InsertAsync(variable);
            }
        }

        var id = (_jobPostRepository.Table as DbSet<DataTier.Entities.JobPost>)
            .FromSqlRaw("SELECT CAST(IDENT_CURRENT('JobPost') AS INT) AS 'id'").Sum(j => j.Id);

        newPayment.Message = "Tạo bài đăng mới #" + ++id;
        await _paymentHistoryRepository.InsertAsync(newPayment);
        await _contextFactory.SaveAllAsync();
        DataTier.Entities.JobPost jobPostAfterUpdate = await GetJobPostById(jobPost.Id);
        return jobPostAfterUpdate;

        return jobPost;
    }

    public async Task<DataTier.Entities.JobPost> UpdateJobPost(DataTier.Entities.JobPost jobPost,
        PayPerHourJob? payPerHourJob,
        PayPerTaskJob? payPerTaskJob,
        ICollection<PinDate>? pinDates,
        DataTier.Entities.PaymentHistory newPayment)
    {
        jobPost.PinDates = pinDates!;
        _jobPostRepository.Modify(jobPost);
        DeletePayment(jobPost.Id, (int)jobPost.PublishedBy);
        jobPost.PaymentHistories = new List<DataTier.Entities.PaymentHistory> { newPayment };
        DeletePayPerTask(jobPost.Id);
        DeletePayPerHour(jobPost.Id);
        if (jobPost.Type.Equals("PayPerHourJob"))
        {
            if (payPerHourJob == null)
                throw new Exception("PayPerHourJob: Empty");

            payPerHourJob.Id = jobPost.Id;
            jobPost.PayPerHourJob = payPerHourJob;
            await _payPerHourRepository.InsertAsync(payPerHourJob);
        }
        else
        {
            if (payPerTaskJob == null)
                throw new Exception("PayPerTaskJob: Empty");

            payPerTaskJob.Id = jobPost.Id;
            jobPost.PayPerTaskJob = payPerTaskJob;
            await _payPerTaskRepository.InsertAsync(payPerTaskJob);
        }

        if (pinDates != null)
        {
            foreach (var variable in pinDates)
            {
                variable.JobPostId = jobPost.Id;
                await _pinDateRepository.InsertAsync(variable);
            }
        }

        newPayment.Message = "Tạo bài đăng mới #" + jobPost.Id;
        await _paymentHistoryRepository.InsertAsync(newPayment);

        await _contextFactory.SaveAllAsync();
        DataTier.Entities.JobPost jobPostAfterUpdate = await GetJobPostById(jobPost.Id);
        return jobPostAfterUpdate;
    }

    public async Task SwitchStatusJob(int jobPostId)
    {
        DataTier.Entities.JobPost? jobPost = await _jobPostRepository.GetByIdAsync(jobPostId);
        if (jobPost == null)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Job post is not existed");
        }

        if (jobPost.Status != (int?)JobPostEnum.JobPostStatus.ShortHandled
            && jobPost.Status != (int?)JobPostEnum.JobPostStatus.Enough)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Invalid Job Post Status");
        }

        if (jobPost.Status == (int?)JobPostEnum.JobPostStatus.ShortHandled)
        {
            jobPost.Status = (int?)JobPostEnum.JobPostStatus.Enough;
        }
        else
        {
            int totalApplied = _appliedRepository.Get(a =>
                a.JobPostId == jobPost.Id && a.Status == (int?)AppliedJobEnum.AppliedJobStatus.Approve).Count();

            if (jobPost.Type.Equals("PayPerHourJob"))
            {
                PayPerHourJob perHourJob = (await _payPerHourRepository.GetByIdAsync(jobPost.Id))!;
                if (perHourJob.MaxFarmer == totalApplied)
                {
                    throw new MException(StatusCodes.Status400BadRequest, "Job post is full farmer");
                }
            }
            else
            {
                if (totalApplied == 1)
                {
                    throw new MException(StatusCodes.Status400BadRequest, "Job post is full farmer");
                }
            }

            jobPost.Status = (int?)JobPostEnum.JobPostStatus.ShortHandled;
        }

        await _contextFactory.SaveAllAsync();
    }

    public async Task<DataTier.Entities.JobPost> EndJobPost(int id)
    {
        DataTier.Entities.JobPost? jobPost = await _jobPostRepository.GetByIdAsync(id);
        if (jobPost == null) throw new Exception("Job Post Not Found");
        if (jobPost.StatusWork == (int?)JobPostEnum.JobPostWorkStatus.Done)
        {
            throw new Exception("Job post has been end");
        }

        jobPost.StatusWork = (int?)JobPostEnum.JobPostWorkStatus.Done;
        Expression<Func<DataTier.Entities.AppliedJob, object>> expAppliedJob = job => job.AppliedByNavigation; 
        IQueryable<DataTier.Entities.AppliedJob> appliedJobs = 
            _appliedRepository.Get(a => 
                a.JobPostId == jobPost.Id 
                && (a.Status == (int?)AppliedJobEnum.AppliedJobStatus.Pending 
                || a.Status == (int?)AppliedJobEnum.AppliedJobStatus.Approve), new []{expAppliedJob});

        List<DataTier.Entities.AppliedJob> appliedJobsList = await appliedJobs.ToListAsync();
        if (jobPost.Type.Equals("PayPerHourJob"))
        {
            foreach (var appliedJob in appliedJobsList)
            {
                if (appliedJob.Status == (int?)AppliedJobEnum.AppliedJobStatus.Pending)
                {
                    appliedJob.Status = (int?)AppliedJobEnum.AppliedJobStatus.Reject;
                    Dictionary<string, string> data = new Dictionary<string, string>()
                    {
                        { "type", "appliedFarmer" }
                    };
                    await PushNotification.SendMessage(appliedJob.AppliedBy.ToString()
                        , $"Ứng tuyển không thành công",
                        $"{appliedJob.AppliedByNavigation.Fullname} đã bị từ chối nhận vào công việc {jobPost.Title}", data);

                }

                if (appliedJob.Status == (int?)AppliedJobEnum.AppliedJobStatus.Approve)
                {
                    appliedJob.Status = (int?)AppliedJobEnum.AppliedJobStatus.End;
                }
            }
        }
        else
        {
            PayPerTaskJob payPerTaskJob = (await _payPerTaskRepository.GetByIdAsync(jobPost.Id))!;
            foreach (var appliedJob in appliedJobsList)
            {
                if (appliedJob.Status == (int?)AppliedJobEnum.AppliedJobStatus.Pending)
                {
                    appliedJob.Status = (int?)AppliedJobEnum.AppliedJobStatus.Reject;
                    Dictionary<string, string> data = new Dictionary<string, string>()
                    {
                        { "type", "appliedFarmer" }
                    };
                    await PushNotification.SendMessage(appliedJob.AppliedBy.ToString()
                        , $"Ứng tuyển không thành công",
                        $"{appliedJob.AppliedByNavigation.Fullname} đã bị từ chối nhận vào công việc {jobPost.Title}", data);

                }

                if (appliedJob.Status == (int?)AppliedJobEnum.AppliedJobStatus.Approve)
                {
                    appliedJob.Status = (int?)AppliedJobEnum.AppliedJobStatus.End;
                    // TODO: them attendance
                    DataTier.Entities.Attendance newAttendance = new DataTier.Entities.Attendance()
                    {
                        Date = DateTime.Today,
                        Salary = payPerTaskJob.Salary,
                        Status = (int?)AttendanceEnum.AttendanceStatus.Present,
                        AccountId = appliedJob.AppliedBy,
                        AppliedJob = appliedJob,
                        CreatedDate = DateTime.Now,
                        AppliedJobId = appliedJob.Id,
                        BonusPoint = 1
                    };
                    
                   await _attendanceRepository.InsertAsync(newAttendance);

                }
            }
        }
        
        await _contextFactory.SaveAllAsync();
        return jobPost;
    }

    public async Task<DataTier.Entities.JobPost> CancelJobPost(int id)
    {
        DataTier.Entities.JobPost jobPost = (await _jobPostRepository.GetByIdAsync(id))!;

        if (jobPost == null)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Job Post is not existed.");
        }

        if (jobPost.Status != (int?)JobPostEnum.JobPostStatus.Approved)
        {
            throw new Exception("Job Post is not approved");
        }

        jobPost.Status = (int?)JobPostEnum.JobPostStatus.Cancel;

        _jobPostRepository.Modify(jobPost);
        ChangePaymentHistory(jobPost.Id, (int)jobPost.PublishedBy);
        await RefundPaymentHistory(jobPost.Id, (int)jobPost.PublishedBy);
        await _contextFactory.SaveAllAsync();

        return jobPost;
    }

    public void DeletePinDate(ICollection<PinDate> pinDates)
    {
        foreach (var variable in pinDates)
        {
            _pinDateRepository.Delete(variable);
        }
    }

    private void ChangePaymentHistory(int jobPostId, int publishId)
    {
        DataTier.Entities.PaymentHistory paymentHistory =
            _paymentHistoryRepository.GetFirstOrDefault(ph => ph.JobPostId == jobPostId && ph.BelongedId == publishId)!;

        paymentHistory.Status = (int?)PaymentHistoryEnum.PaymentHistoryStatus.Rejected;
    }

    public IQueryable GetPinDateByJobPost(int id)
    {
        var result = _pinDateRepository.Get(p => p.JobPostId == id);
        return result;
    }

    private void ApprovePaymentHistory(int jobPostId, int publishId)
    {
        DataTier.Entities.PaymentHistory paymentHistory =
            _paymentHistoryRepository.GetFirstOrDefault(ph => ph.JobPostId == jobPostId && ph.BelongedId == publishId)!;
        DataTier.Entities.Account account = _accountRepository.GetById(publishId)!;

        if (paymentHistory.UsedPoint - paymentHistory.EarnedPoint > account.Point)
        {
            throw new Exception("Point in account not enough");
        }

        if (paymentHistory.ActualPrice > account.Balance)
        {
            throw new Exception("Balance is not enough");
        }

        account.Balance -= paymentHistory.ActualPrice;
        account.Point -= paymentHistory.UsedPoint;
        account.Point += paymentHistory.EarnedPoint;
        paymentHistory.Status = (int?)PaymentHistoryEnum.PaymentHistoryStatus.Paid;
    }

    private async Task RefundPaymentHistory(int jobPostId, int publishId)
    {
        DataTier.Entities.PaymentHistory paymentHistory =
            _paymentHistoryRepository.GetFirstOrDefault(ph => ph.JobPostId == jobPostId && ph.BelongedId == publishId)!;
        DataTier.Entities.Account account = _accountRepository.GetById(publishId)!;
        //-- Refund money
        account.Balance += paymentHistory.ActualPrice;
        account.Point += paymentHistory.UsedPoint;
        account.Point -= paymentHistory.EarnedPoint;
        //-- New Refund Payment
        DataTier.Entities.PaymentHistory refundPayment = new DataTier.Entities.PaymentHistory
        {
            JobPostId = paymentHistory.JobPostId,
            CreatedBy = paymentHistory.CreatedBy,
            Status = (int?)PaymentHistoryEnum.PaymentHistoryStatus.Paid,
            CreatedDate = DateTime.Now,
            ActualPrice = paymentHistory.ActualPrice,
            BalanceFluctuation = paymentHistory.BalanceFluctuation * -1,
            Balance = account.Balance,
            EarnedPoint = 0,
            UsedPoint = 0,
            BelongedId = paymentHistory.BelongedId,
            Message = "Hoàn tiền cho bài đăng mới #" + jobPostId,
            TotalPinDay = paymentHistory.TotalPinDay,
            PostTypePrice = paymentHistory.PostTypePrice,
            PointPrice = (paymentHistory.PointPrice)
        };

        await _paymentHistoryRepository.InsertAsync(refundPayment);
    }

    public async Task<DataTier.Entities.JobPost> DeleteJobPost(int id)
    {
        DataTier.Entities.JobPost? jobPost = await _jobPostRepository.GetByIdAsync(id);

        if (jobPost == null)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Job Post is not existed.");
        }

        jobPost.Status = (int?)JobPostEnum.JobPostStatus.Deleted;

        _jobPostRepository.Modify(jobPost);

        IQueryable<PinDate> queryPin = (IQueryable<PinDate>)GetPinDateByJobPost(jobPost.Id);
        ICollection<PinDate> list = queryPin.ToList();
        DeletePinDate(list);
        DeletePayment(jobPost.Id, ((int)jobPost.PublishedBy)!);

        await _contextFactory.SaveAllAsync();

        return jobPost;
    }

    public async Task<DataTier.Entities.JobPost> ApproveJobPost(int jobPostId, int approvedBy)
    {
        DataTier.Entities.JobPost jobPost = (await _jobPostRepository.GetByIdAsync(jobPostId))!;

        if (jobPost == null)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Job Post is not existed.");
        }

        IQueryable<PinDate> queryPin = (IQueryable<PinDate>)GetPinDateByJobPost(jobPost.Id);
        if (jobPost.Status == (int?)JobPostEnum.JobPostStatus.Approved)
        {
            throw new Exception("JobPost has been approved");
        }

        ICollection<PinDate> list = queryPin.ToList();
        jobPost.Status = (int?)JobPostEnum.JobPostStatus.Approved;
        if (jobPost.PublishedDate.Value.Date == DateTime.Today.Date)
        {
            jobPost.Status = (int?)JobPostEnum.JobPostStatus.ShortHandled;
        }

        jobPost.ApprovedDate = DateTime.Now;
        jobPost.ApprovedBy = approvedBy;
        foreach (var pinData in list)
        {
            pinData.Status = 1;
        }

        ApprovePaymentHistory(jobPostId, (int)jobPost.PublishedBy);
        _jobPostRepository.Modify(jobPost);
        await _contextFactory.SaveAllAsync();

        return jobPost;
    }

    public async Task<DataTier.Entities.JobPost> RejectJobPost(RejectJobPost rejectJobPost, int approvedBy)
    {
        DataTier.Entities.JobPost? jobPost = await _jobPostRepository.GetByIdAsync(rejectJobPost.Id);

        if (jobPost == null)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Job Post is not existed.");
        }

        jobPost.ApprovedBy = approvedBy;
        jobPost.ApprovedDate = DateTime.Now;
        jobPost.ReasonReject = rejectJobPost.ReasonReject;
        jobPost.Status = (int?)JobPostEnum.JobPostStatus.Reject;
        _jobPostRepository.Modify(jobPost);
        IQueryable<PinDate> queryPin = (IQueryable<PinDate>)GetPinDateByJobPost(jobPost.Id);
        ICollection<PinDate> list = queryPin.ToList();
        DeletePinDate(list);
        ChangePaymentHistory(jobPost.Id, ((int)jobPost.PublishedBy)!);

        await _contextFactory.SaveAllAsync();

        return jobPost;
    }

    public async Task<DataTier.Entities.JobPost> StartJob(int jobPostId)
    {
        DataTier.Entities.JobPost jobPost = (await _jobPostRepository.GetByIdAsync(jobPostId))!;

        if (jobPost == null)
        {
            throw new Exception("Job Post is not existed");
        }

        if (jobPost.StatusWork == (int?)JobPostEnum.JobPostWorkStatus.Started)
        {
            throw new Exception("Job post has been started");
        }

        jobPost.StatusWork = (int?)JobPostEnum.JobPostWorkStatus.Started;
        // jobPost.Status = (int?)JobPostEnum.JobPostStatus.ShortHandled;
        _jobPostRepository.Modify(jobPost);

        await _contextFactory.SaveAllAsync();

        return jobPost;
    }

    public async Task ExtendMaxFarmer(int jobPostId, int maxFarmer)
    {
        DataTier.Entities.JobPost _jobPost = (await _jobPostRepository.GetByIdAsync(jobPostId))!;

        PayPerHourJob payPerHour = (await _payPerHourRepository.GetByIdAsync(jobPostId))!;
        if (payPerHour.MaxFarmer >= maxFarmer)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Max farmer must lager than old max farmer");
        }

        payPerHour.MaxFarmer = maxFarmer;
        _jobPost.Status = (int?)JobPostEnum.JobPostStatus.ShortHandled;
        await _contextFactory.SaveAllAsync();
    }

    public async Task<DataTier.Entities.JobPost> ExtendJobPostForLandowner(
        DataTier.Entities.JobPost jobPost,
        DataTier.Entities.PaymentHistory newPayment,
        DataTier.Entities.Account account)
    {
        _jobPostRepository.Modify(jobPost);
        _accountRepository.Modify(account);

        await _paymentHistoryRepository.InsertAsync(newPayment);

        await _contextFactory.SaveAllAsync();

        return jobPost;
    }

    public async Task<int> TotalPinDate(int jobPostId)
    {
        Expression<Func<DataTier.Entities.JobPost, object>> expPinDate = post => post.PinDates;
        DataTier.Entities.JobPost jobPost = (await _jobPostRepository.GetFirstOrDefaultAsync(
            j => j.Id == jobPostId && j.Status != (int?)JobPostEnum.JobPostStatus.Deleted,
            new[] { expPinDate }))!;

        IEnumerable<PinDate> listPin = jobPost.PinDates.Where(p => p.Status == 1);
        return listPin.Count();
    }

    public async Task<DataTier.Entities.JobPost> UpdateStartDate(int jobPostId, DateTime startDate)
    {
        DataTier.Entities.JobPost jobPost = await _jobPostRepository.GetByIdAsync(jobPostId);
        jobPost.StartJobDate = startDate;

        await _contextFactory.SaveAllAsync();
        return jobPost;
    }

    public async Task PostingJob()
    {
        IQueryable<DataTier.Entities.JobPost> query = _jobPostRepository.Get(j =>
            j.Status == (int?)JobPostEnum.JobPostStatus.Approved && j.PublishedDate.Value.Date.Equals(DateTime.Today));
        
        List<DataTier.Entities.JobPost> listPosting = await query.ToListAsync();
        foreach (var jobPost in listPosting)
        {
            jobPost.Status = (int?)JobPostEnum.JobPostStatus.ShortHandled;
            _jobPostRepository.Modify(jobPost);
            Console.WriteLine($"Posted: #{jobPost.Id} - {DateTime.Now}",
                Console.BackgroundColor == ConsoleColor.Yellow);
        }
        
        await _contextFactory.SaveAllAsync();
    }

    public async Task ExpiredJob()
    {
        // IQueryable<DataTier.Entities.JobPost> query = _jobPostRepository.Get(j =>
        //     j.Status == (int?)JobPostEnum.JobPostStatus.Posted &&
        //     j.PublishedDate!.Value.Date.AddDays((double)j.NumPublishDay) == DateTime.Today);
        //
        // List<DataTier.Entities.JobPost> listExpired = query.ToList();
        // foreach (var jobPost in listExpired)
        // {
        //     jobPost.Status = (int?)JobPostEnum.JobPostStatus.Expired;
        //     Console.WriteLine($"Expired: #{jobPost.Id} - {DateTime.Now}",
        //         Console.BackgroundColor == ConsoleColor.Yellow);
        // }
        //
        // await _contextFactory.SaveAllAsync();
    }

    public async Task StartJob()
    {
        IQueryable<DataTier.Entities.JobPost> query = _jobPostRepository.Get(j =>
            (j.Status == (int?)JobPostEnum.JobPostStatus.ShortHandled || j.Status == (int?)JobPostEnum.JobPostStatus.Enough)
            && j.StatusWork != (int?)JobPostEnum.JobPostWorkStatus.Started
            && j.StartJobDate.Value.Date == DateTime.Today);

        List<DataTier.Entities.JobPost> listStart = query.ToList();
        foreach (var jobPost in listStart)
        {
            jobPost.StatusWork = (int?)JobPostEnum.JobPostWorkStatus.Started;
            Console.WriteLine($"Start: #{jobPost.Id} - {DateTime.Now}",
                Console.BackgroundColor == ConsoleColor.Yellow);
        }

        await _contextFactory.SaveAllAsync();
    }

    public async Task ExpiredApproveJob()
    {
        IQueryable<DataTier.Entities.JobPost> query = _jobPostRepository.Get(j =>
            j.Status == (int?)JobPostEnum.JobPostStatus.Pending &&
            j.PublishedDate!.Value.Date < DateTime.Today);
        
        List<DataTier.Entities.JobPost> listOutOfDate = query.ToList();
        foreach (var jobPost in listOutOfDate)
        {
            jobPost.Status = (int?)JobPostEnum.JobPostStatus.OutOfDate;
            DataTier.Entities.PaymentHistory? paymentHistories = await
                _paymentHistoryRepository.GetFirstOrDefaultAsync(p =>
                    p.JobPostId == jobPost.Id && p.BelongedId == jobPost.PublishedBy);
            _paymentHistoryRepository.Delete(paymentHistories!);
            Console.WriteLine($"Expired: #{jobPost.Id} - {DateTime.Now}",
                Console.BackgroundColor == ConsoleColor.Yellow);
        }
        
        await _contextFactory.SaveAllAsync();
    }

    public async Task SaveChange()
    {
        await _contextFactory.SaveAllAsync();
    }

    private void DeletePayPerHour(int jobPostId)
    {
        PayPerHourJob? payPerHourJob = _payPerHourRepository.GetFirstOrDefault(pph => pph.Id == jobPostId);
        if (payPerHourJob != null)
        {
            _payPerHourRepository.Delete(payPerHourJob);
        }
    }

    private void DeletePayPerTask(int jobPostId)
    {
        PayPerTaskJob? payPerTaskJob = _payPerTaskRepository.GetFirstOrDefault(ppt => ppt.Id == jobPostId);
        if (payPerTaskJob != null)
        {
            _payPerTaskRepository.Delete(payPerTaskJob);
        }
    }

    private void DeletePayment(int jobPostId, int publishId)
    {
        DataTier.Entities.PaymentHistory? paymentHistory = _paymentHistoryRepository
            .Get(ph => ph.JobPostId == jobPostId && ph.BelongedId == publishId)
            .FirstOrDefault();
        if (paymentHistory != null)
        {
            _paymentHistoryRepository.Delete(paymentHistory);
        }
    }
}