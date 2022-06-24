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
    }

    public IQueryable<DataTier.Entities.JobPost> GetJobPosts(int? publishBy = null)
    {
        Expression<Func<DataTier.Entities.JobPost, object>> expHours = post => post.PayPerHourJob; 
        Expression<Func<DataTier.Entities.JobPost, object>> expTask = post => post.PayPerTaskJob;
        bool flag = publishBy != null;
        IQueryable<DataTier.Entities.JobPost> query = _jobPostRepository.Get(
            (flag ? post => post.PublishedBy == publishBy : null) ,
            new []{expHours, expTask});
        return query;
    }


    public IQueryable<DataTier.Entities.JobPost> GetPinPost()
    {
        Expression<Func<DataTier.Entities.JobPost, object>> expHours = post => post.PayPerHourJob; 
        Expression<Func<DataTier.Entities.JobPost, object>> expTask = post => post.PayPerTaskJob;
        IQueryable<PinDate> pinDates = _pinDateRepository.Get(p => p.PinDate1.Date.Equals(DateTime.Today) && p.Status == 1);
        var postId = pinDates.Select(p=>p.JobPostId);
        IQueryable<DataTier.Entities.JobPost> jobPosts = _jobPostRepository.Get(null, new []{expHours, expTask});
        jobPosts = jobPosts.Where(p => postId.Contains(p.Id));
        return jobPosts;
    }

    public IQueryable<DataTier.Entities.JobPost> GetInProgressJobPost()
    {
        Expression<Func<DataTier.Entities.JobPost, object>> expAppliedJob = post => post.AppliedJobs;
        IQueryable<DataTier.Entities.JobPost> inProgress = _jobPostRepository.Get(post => post.StatusWork == (int?)JobPostEnum.JobPostWorkStatus.Start, new []{expAppliedJob});
        return inProgress;
    }


    public async Task<DataTier.Entities.JobPost> GetJobPostById(int id)
    {
        Expression<Func<DataTier.Entities.JobPost, object>> expHours = post => post.PayPerHourJob; 
        Expression<Func<DataTier.Entities.JobPost, object>> expTask = post => post.PayPerTaskJob;
        DataTier.Entities.JobPost jobPost = (await _jobPostRepository.GetFirstOrDefaultAsync(j => j.Id == id, new []{expHours, expTask}))!;
        return jobPost;
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
            foreach (var VARIABLE in pinDates)
            {
                VARIABLE.JobPostId = jobPost.Id;
                await _pinDateRepository.InsertAsync(VARIABLE);
            }
        }
        
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
        jobPost.PinDates = pinDates;
        _jobPostRepository.Modify(jobPost);
        ChangePaymentHistory(jobPost.Id, (int)jobPost.PublishedBy);
        jobPost.PaymentHistories = new List<DataTier.Entities.PaymentHistory> { newPayment };
        if (jobPost.Type.Equals("PayPerHourJob"))
        {
            if (payPerHourJob == null) throw new Exception("PayPerHourJob: Empty");
            jobPost.PayPerHourJob = payPerHourJob;
            payPerHourJob.Id = jobPost.Id;
            _payPerHourRepository.Modify(payPerHourJob);
        }
        else
        {
            if (payPerTaskJob == null) throw new Exception("PayPerTaskJob: Empty");
            jobPost.PayPerTaskJob = payPerTaskJob;
            payPerTaskJob.Id = jobPost.Id;
            _payPerTaskRepository.Modify(payPerTaskJob);
        }

        if (pinDates != null)
        {
            foreach (var VARIABLE in pinDates)
            {
                VARIABLE.JobPostId = jobPost.Id;
                await _pinDateRepository.InsertAsync(VARIABLE);
            }
        }
        await _paymentHistoryRepository.InsertAsync(newPayment);
        
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

        if (jobPost.Status != (int?)JobPostEnum.JobPostStatus.Posted)
        {
            throw new Exception("Job Post is not posted");
        }
        
        jobPost.Status = (int?)JobPostEnum.JobPostStatus.Cancel;
        
        _jobPostRepository.Modify(jobPost);

        await _contextFactory.SaveAllAsync();

        return jobPost;
    }

    public void DeletePinDate(ICollection<PinDate> pinDates)
    {
        foreach (var VARIABLE in pinDates)
        {
            _pinDateRepository.Delete(VARIABLE);
        }
    }

    public void ChangePaymentHistory(int jobPostId, int publishId)
    {
        DataTier.Entities.PaymentHistory paymentHistory =
            _paymentHistoryRepository.GetFirstOrDefault(ph => ph.JobPostId == jobPostId && ph.BelongedId == publishId)!;

        paymentHistory.Status = (int?)PaymentHistoryEnum.PaymentHistoryStatus.Reject;
    }

    public IQueryable GetPinDateByJobPost(int id)
    {
        var result = _pinDateRepository.Get(p => p.JobPostId == id);
        return result;
    }

    public void ApprovePaymentHistory(int jobPostId, int publishId)
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
        if (jobPost.Status == (int?)JobPostEnum.JobPostStatus.Posted)
        {
            throw new Exception("JobPost has been approved");
        }
        ICollection<PinDate> list = queryPin.ToList();
        jobPost.Status = (int?)JobPostEnum.JobPostStatus.Posted;
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

        if (jobPost.StatusWork == (int?)JobPostEnum.JobPostWorkStatus.Start)
        {
            throw new Exception("Job post has been started");
        }
        /// 
        
        jobPost.StatusWork = (int?)JobPostEnum.JobPostWorkStatus.Start;
        _jobPostRepository.Modify(jobPost);

        await _contextFactory.SaveAllAsync();

        return jobPost;
    }
}