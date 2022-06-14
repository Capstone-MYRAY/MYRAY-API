using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

    public JobPostRepository(IDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
        var context = _contextFactory.GetContext<MYRAYContext>();
        _jobPostRepository = context.GetRepository<DataTier.Entities.JobPost>()!;
        _payPerHourRepository = context.GetRepository<PayPerHourJob>()!;
        _payPerTaskRepository = context.GetRepository<PayPerTaskJob>()!;
        _pinDateRepository = context.GetRepository<PinDate>()!;
    }

    public IQueryable<DataTier.Entities.JobPost> GetJobPosts(int? publishBy = null)
    {
        Expression<Func<DataTier.Entities.JobPost, object>> expHours = post => post.PayPerHourJob; 
        Expression<Func<DataTier.Entities.JobPost, object>> expTask = post => post.PayPerTaskJob;
        bool flag = publishBy != null;
        IQueryable<DataTier.Entities.JobPost> query = _jobPostRepository.Get(flag? post => post.PublishedBy == publishBy : null , new []{expHours, expTask});
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
        ICollection<PinDate>? pinDates)
    {
        if (pinDates != null)
        {
            jobPost.PinDates = pinDates;
        }
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

        
        await _contextFactory.SaveAllAsync();

        return jobPost;
    }

    public async Task<DataTier.Entities.JobPost> UpdateJobPost(DataTier.Entities.JobPost jobPost, 
        PayPerHourJob? payPerHourJob, 
        PayPerTaskJob? payPerTaskJob, 
        ICollection<PinDate>? pinDates)
    {
        jobPost.PinDates = pinDates;
        
        _jobPostRepository.Modify(jobPost);

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

    public IQueryable GetPinDateByJobPost(int id)
    {
        var result = _pinDateRepository.Get(p => p.JobPostId == id);
        return result;
    }
    
    public async Task<DataTier.Entities.JobPost> DeleteJobPost(int id)
    {
        DataTier.Entities.JobPost? jobPost = await _jobPostRepository.GetByIdAsync(id);

        if (jobPost == null)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Job Post is not existed.");
        }

        jobPost.Status = (int?)JobPostEnum.JobPostStatus.Cancle;
        
        _jobPostRepository.Modify(jobPost);

        IQueryable<PinDate> queryPin = (IQueryable<PinDate>)GetPinDateByJobPost(jobPost.Id);
        ICollection<PinDate> list = queryPin.ToList();
        DeletePinDate(list);
        
        await _contextFactory.SaveAllAsync();

        return jobPost;
    }
}