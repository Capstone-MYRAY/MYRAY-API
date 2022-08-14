using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MYRAY.Business.Enums;
using MYRAY.Business.Exceptions;
using MYRAY.Business.Repositories.Interface;
using MYRAY.DataTier.Entities;
using static MYRAY.Business.Services.Notification.PushNotification;

namespace MYRAY.Business.Repositories.AppliedJob;

public class AppliedJobRepository : IAppliedJobRepository
{
    private readonly IDbContextFactory _contextFactory;
    private readonly IBaseRepository<DataTier.Entities.AppliedJob> _appliedJobRepository;
    private readonly IBaseRepository<DataTier.Entities.JobPost> _jobPostRepository;
    private readonly IBaseRepository<PayPerHourJob> _payPerHourJob;

    public AppliedJobRepository(IDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
        _appliedJobRepository =
            _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.AppliedJob>()!;
        _jobPostRepository = _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.JobPost>()!;
        _payPerHourJob = _contextFactory.GetContext<MYRAYContext>().GetRepository<PayPerHourJob>()!;
    }

    public IQueryable<DataTier.Entities.AppliedJob> GetAppliedJobs(int jobId,
        AppliedJobEnum.AppliedJobStatus? status = null)
    {
        IQueryable<DataTier.Entities.AppliedJob> query = _appliedJobRepository.Get(ap =>
            ap.JobPostId == jobId && (status != null ? ap.Status == (int?)status : true));
        return query;
    }

    public IQueryable<DataTier.Entities.AppliedJob> GetAppliedJobsExceptPending(int jobId)
    {
        IQueryable<DataTier.Entities.AppliedJob> query = _appliedJobRepository.Get(ap => ap.JobPostId == jobId &&
            (ap.Status != (int?)AppliedJobEnum.AppliedJobStatus.Pending
             && ap.Status != (int?)AppliedJobEnum.AppliedJobStatus.Reject));
        return query;
    }


    public IQueryable<DataTier.Entities.AppliedJob> GetAllAppliedJobs(int landownerId,
        AppliedJobEnum.AppliedJobStatus? status = null)
    {
        IQueryable<DataTier.Entities.AppliedJob> query = _appliedJobRepository.Get(aj =>
            aj.JobPost.PublishedBy == landownerId && (status != null ? aj.Status == (int?)status : true));
        return query;
    }

    public IQueryable<DataTier.Entities.AppliedJob> GetAppliedJobsFarmer(int farmerId,
        AppliedJobEnum.AppliedJobStatus? status = null)
    {
        IQueryable<DataTier.Entities.AppliedJob> query = _appliedJobRepository.Get(ap =>
            ap.AppliedBy == farmerId && (status == null || ap.Status == (int?)status));
        return query;
    }

    public async Task<DataTier.Entities.AppliedJob> GetByJobAndAccount(int jobPostId, int accountId)
    {
        DataTier.Entities.AppliedJob appliedJob =
            await _appliedJobRepository.GetFirstOrDefaultAsync(
                a => a.JobPostId == jobPostId && a.AppliedBy == accountId);
        if (appliedJob == null)
        {
            throw new Exception("Do not Applied Job");
        }

        return appliedJob;
    }

    public async Task<DataTier.Entities.AppliedJob> ApplyJob(int jobId, int appliedBy)
    {
        DataTier.Entities.AppliedJob checkExisted =
            (await _appliedJobRepository.GetFirstOrDefaultAsync(
                ap => ap.JobPostId == jobId && ap.AppliedBy == appliedBy))!;
        if (checkExisted != null) throw new Exception("You already applied for this job");
        var appliedJob = new DataTier.Entities.AppliedJob
        {
            AppliedBy = appliedBy,
            AppliedDate = DateTime.Now,
            JobPostId = jobId,
            Status = (int?)AppliedJobEnum.AppliedJobStatus.Pending
        };
        await _appliedJobRepository.InsertAsync(appliedJob);

        await _contextFactory.SaveAllAsync();

        return appliedJob;
    }

    public async Task<DataTier.Entities.AppliedJob> CancelApply(int jobId, int appliedBy)
    {
        DataTier.Entities.AppliedJob? appliedJob =
            await _appliedJobRepository.GetFirstOrDefaultAsync(aj =>
                aj.JobPostId == jobId && aj.AppliedBy == appliedBy);

        if (appliedJob == null)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Applied Job is not existed.");
        }

        _appliedJobRepository.Delete(appliedJob);

        await _contextFactory.SaveAllAsync();

        return appliedJob;
    }

    public async Task<DataTier.Entities.AppliedJob> ApproveJob(int appliedJobId)
    {
        Expression<Func<DataTier.Entities.AppliedJob, object>> expApplied = job => job.AppliedByNavigation;
        DataTier.Entities.AppliedJob? appliedJob =
            await _appliedJobRepository.GetFirstOrDefaultAsync(a => a.Id == appliedJobId, new[] { expApplied });
        if (appliedJob == null)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Applied Job is not existed.");
        }

        if (appliedJob.Status != (int?)AppliedJobEnum.AppliedJobStatus.Pending)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Applied job is not pending");
        }

        appliedJob.Status = (int)AppliedJobEnum.AppliedJobStatus.Approve;
        appliedJob.ApprovedDate = DateTime.Now;
        _appliedJobRepository.Modify(appliedJob);
        DataTier.Entities.JobPost? jobPost = await _jobPostRepository.GetByIdAsync(appliedJob.JobPostId);
        bool isEnough = false;
        if (jobPost!.Type.Equals("PayPerTaskJob"))
        {
            IQueryable<DataTier.Entities.AppliedJob> rejectList = _appliedJobRepository.Get(a =>
                    a.JobPostId == appliedJob.JobPostId
                    && a.AppliedBy != appliedJob.AppliedBy
                    && a.Status == (int?)AppliedJobEnum.AppliedJobStatus.Pending,
                new[] { expApplied });
            List<DataTier.Entities.AppliedJob> listAppliedJob = await rejectList.ToListAsync();
            listAppliedJob.ForEach(async la =>
            {
                la.Status = (int)AppliedJobEnum.AppliedJobStatus.Reject;
                // // Sent noti
                Dictionary<string, string> data = new Dictionary<string, string>()
                {
                    { "type", "appliedFarmer" }
                };
                await SendMessage(la.AppliedBy.ToString()
                    , $"Ứng tuyển không thành công",
                    $"{la.AppliedByNavigation.Fullname} đã bị từ chối nhận vào công việc {la.JobPost.Title}", data);
            });

            isEnough = true;
        }

        if (jobPost!.Type.Equals("PayPerHourJob"))
        {
            PayPerHourJob perHourJob = (await _payPerHourJob.GetByIdAsync(jobPost.Id))!;
            int totalApplied = _appliedJobRepository.Get(a =>
                a.JobPostId == jobPost.Id && a.Status == (int?)AppliedJobEnum.AppliedJobStatus.Approve).Count();
            if (totalApplied >= perHourJob.MaxFarmer - 1)
            {
                #region RejectApplie_atManyJob

                IQueryable<DataTier.Entities.AppliedJob> rejectList = _appliedJobRepository.Get(a =>
                        a.JobPost.Type.Equals("PayPerHourJob")
                        && a.Id != appliedJob.Id
                        && a.AppliedBy == appliedJob.AppliedBy
                        && a.Status == (int?)AppliedJobEnum.AppliedJobStatus.Pending,
                    new[] { expApplied });
                List<DataTier.Entities.AppliedJob> listAppliedJob = await rejectList.ToListAsync();
                listAppliedJob.ForEach(async la =>
                {
                    la.Status = (int)AppliedJobEnum.AppliedJobStatus.Reject;
                    // // Sent noti
                    Dictionary<string, string> data = new Dictionary<string, string>()
                    {
                        { "type", "appliedFarmer" }
                    };
                    await SendMessage(la.AppliedBy.ToString()
                        , $"Ứng tuyển không thành công",
                        $"Hệ thống đã tự động hủy yêu cầu xin vào công việc {la.JobPost.Title} của bạn", data);
                });

                #endregion

                #region Reject_Appied_Of_Job
                IQueryable<DataTier.Entities.AppliedJob> rejectListJob = _appliedJobRepository.Get(a =>
                        a.JobPost.Type.Equals("PayPerHourJob")
                        && a.Id != appliedJob.Id
                        && a.JobPostId == jobPost.Id
                        && a.Status == (int?)AppliedJobEnum.AppliedJobStatus.Pending,
                    new[] { expApplied });
                
                List<DataTier.Entities.AppliedJob> listAppliedJob1 = await rejectListJob.ToListAsync();
                listAppliedJob1.ForEach(async la =>
                {
                    la.Status = (int)AppliedJobEnum.AppliedJobStatus.Reject;
                    // // Sent noti
                    Dictionary<string, string> data = new Dictionary<string, string>()
                    {
                        { "type", "appliedFarmer" }
                    };
                    await SendMessage(la.AppliedBy.ToString()
                        , $"Ứng tuyển không thành công",
                        $"{la.AppliedByNavigation.Fullname} đã bị từ chối nhận vào công việc {la.JobPost.Title}", data);
                });
                
                #endregion

                isEnough = true;
            }

            if (isEnough)
            {
                jobPost.Status = (int?)JobPostEnum.JobPostStatus.Enough;
            }
        }

        await _contextFactory.SaveAllAsync();

        return appliedJob;
    }

    public async Task<DataTier.Entities.AppliedJob> RejectJob(int appliedJobId)
    {
        Expression<Func<DataTier.Entities.AppliedJob, object>> expApplied = job => job.AppliedByNavigation;
        Expression<Func<DataTier.Entities.AppliedJob, object>> expJobPost = job => job.JobPost;
        DataTier.Entities.AppliedJob? appliedJob =
            await _appliedJobRepository.GetFirstOrDefaultAsync(a => a.Id == appliedJobId,
                new[] { expApplied, expJobPost });

        if (appliedJob == null)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Applied Job is not existed.");
        }

        appliedJob.Status = (int)AppliedJobEnum.AppliedJobStatus.Reject;
        _appliedJobRepository.Modify(appliedJob);

        await _contextFactory.SaveAllAsync();

        return appliedJob;
    }

    public async Task<DataTier.Entities.AppliedJob?> CheckApplied(int jobPostId, int appliedBy)
    {
        DataTier.Entities.AppliedJob? appliedJob = await
            _appliedJobRepository.GetFirstOrDefaultAsync(al => al.JobPostId == jobPostId && al.AppliedBy == appliedBy);
        return appliedJob;
    }

    public async Task<bool> CheckAppliedHourJob(int farmerId)
    {
        IQueryable<DataTier.Entities.AppliedJob> query = _appliedJobRepository.Get(a => a.AppliedBy == farmerId
            && a.JobPost.Type.Equals("PayPerHourJob")
            && (a.Status == (int?)AppliedJobEnum.AppliedJobStatus.Pending
                || a.Status == (int?)AppliedJobEnum.AppliedJobStatus.Approve)
        );

        DataTier.Entities.AppliedJob? appliedJob = await query.FirstOrDefaultAsync();
        return appliedJob != null;
    }
}