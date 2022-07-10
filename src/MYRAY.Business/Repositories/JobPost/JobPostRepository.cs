using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MYRAY.Business.DTOs.JobPost;
using MYRAY.Business.Enums;
using MYRAY.Business.Exceptions;
using MYRAY.Business.Repositories.Interface;
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
    }

    public IQueryable<DataTier.Entities.JobPost> GetJobPosts(int? publishBy = null)
    {
        Expression<Func<DataTier.Entities.JobPost, object>> expHours = post => post.PayPerHourJob;
        Expression<Func<DataTier.Entities.JobPost, object>> expTask = post => post.PayPerTaskJob;
        bool flag = publishBy != null;
        IQueryable<DataTier.Entities.JobPost> query = _jobPostRepository.Get(post =>
                post.Status != (int?)JobPostEnum.JobPostStatus.Deleted &&
                (!flag || post.PublishedBy == publishBy),
            new[] { expHours, expTask });
        return query;
    }
    
    public IQueryable<DataTier.Entities.JobPost> GetPinPost()
    {
        Expression<Func<DataTier.Entities.JobPost, object>> expHours = post => post.PayPerHourJob;
        Expression<Func<DataTier.Entities.JobPost, object>> expTask = post => post.PayPerTaskJob;
        IQueryable<PinDate> pinDates =
            _pinDateRepository.Get(p => p.PinDate1.Date.Equals(DateTime.Today) && p.Status == 1);
        var postId = pinDates.Select(p => p.JobPostId);
        IQueryable<DataTier.Entities.JobPost> jobPosts = _jobPostRepository.Get(null, new[] { expHours, expTask });
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
        DataTier.Entities.JobPost jobPost = (await _jobPostRepository.GetFirstOrDefaultAsync(
            j => j.Id == id && j.Status != (int?)JobPostEnum.JobPostStatus.Deleted,
            new[] { expHours, expTask, expGarden, expTree }))!;
        return jobPost;
    }

    public async Task<PayPerHourJob> GetPayPerHourJob(int jobPostId)
    {
        return await _payPerHourRepository.GetFirstOrDefaultAsync(pph => pph.Id == jobPostId);
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

        newPayment.Message = "Tạo bài đăng mới #" + jobPost.Id;
        await _paymentHistoryRepository.InsertAsync(newPayment);


        await _contextFactory.SaveAllAsync();

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
        DataTier.Entities.JobPost jobPostAfterUpdate = await _jobPostRepository.Get(j => j.Id == jobPost.Id)
            .Include(ji => ji.TreeJobs)
            .ThenInclude(jt => jt.TreeType)
            .FirstAsync();
        return jobPostAfterUpdate;
    }

    public async Task<DataTier.Entities.JobPost> CancelJobPost(int id)
    {
        DataTier.Entities.JobPost jobPost = (await _jobPostRepository.GetByIdAsync(id))!;

        if (jobPost == null)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Job Post is not existed.");
        }

        if (jobPost.Status != (int?)JobPostEnum.JobPostStatus.Posted)
        {
            throw new Exception("Job Post is not posted");
        }

        jobPost.Status = (int?)JobPostEnum.JobPostStatus.Cancel;

        _jobPostRepository.Modify(jobPost);
        ChangePaymentHistory(jobPost.Id, (int)jobPost.PublishedBy);
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

    public void ChangePaymentHistory(int jobPostId, int publishId)
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
        ChangePaymentHistory(jobPost.Id, ((int)jobPost.PublishedBy)!);

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
        _jobPostRepository.Modify(jobPost);

        await _contextFactory.SaveAllAsync();

        return jobPost;
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

    public async Task PostingJob()
    {
        IQueryable<DataTier.Entities.JobPost> query = _jobPostRepository.Get(j =>
            j.Status == (int?)JobPostEnum.JobPostStatus.Approved && j.PublishedDate.Value.Date.Equals(DateTime.Today));

        List<DataTier.Entities.JobPost> listPosting = await query.ToListAsync();
        foreach (var jobPost in listPosting)
        {
            jobPost.Status = (int?)JobPostEnum.JobPostStatus.Posted;
            _jobPostRepository.Modify(jobPost);
            Console.WriteLine($"Posted: #{jobPost.Id} - {DateTime.Now}",
                Console.BackgroundColor == ConsoleColor.Yellow);
        }

        await _contextFactory.SaveAllAsync();
    }

    public async Task ExpiredJob()
    {
        IQueryable<DataTier.Entities.JobPost> query = _jobPostRepository.Get(j =>
            j.Status == (int?)JobPostEnum.JobPostStatus.Posted &&
            j.PublishedDate!.Value.Date.AddDays((double)j.NumPublishDay) == DateTime.Today);

        List<DataTier.Entities.JobPost> listExpired = query.ToList();
        foreach (var jobPost in listExpired)
        {
            jobPost.Status = (int?)JobPostEnum.JobPostStatus.Expired;
            Console.WriteLine($"Expired: #{jobPost.Id} - {DateTime.Now}",
                Console.BackgroundColor == ConsoleColor.Yellow);
        }

        await _contextFactory.SaveAllAsync();
    }

    public async Task StartJob()
    {
        IQueryable<DataTier.Entities.JobPost> query = _jobPostRepository.Get(j =>
            j.Status == (int?)JobPostEnum.JobPostStatus.Posted 
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
        DataTier.Entities.PaymentHistory? paymentHistory = _paymentHistoryRepository.Get(ph => ph.JobPostId == jobPostId && ph.BelongedId == publishId)
            .FirstOrDefault();
        if (paymentHistory != null)
        {
            _paymentHistoryRepository.Delete(paymentHistory);
        }
    }
}