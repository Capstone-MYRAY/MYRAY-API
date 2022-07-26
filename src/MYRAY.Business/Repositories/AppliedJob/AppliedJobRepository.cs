using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MYRAY.Business.Enums;
using MYRAY.Business.Exceptions;
using MYRAY.Business.Repositories.Interface;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.AppliedJob;

public class AppliedJobRepository : IAppliedJobRepository
{
   private readonly IDbContextFactory _contextFactory;
   private readonly IBaseRepository<DataTier.Entities.AppliedJob> _appliedJobRepository;

   public AppliedJobRepository(IDbContextFactory contextFactory)
   {
      _contextFactory = contextFactory;
      _appliedJobRepository = _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.AppliedJob>()!;
   }
   
   public IQueryable<DataTier.Entities.AppliedJob> GetAppliedJobs(int jobId, AppliedJobEnum.AppliedJobStatus? status = null)
   {
      IQueryable<DataTier.Entities.AppliedJob> query = _appliedJobRepository.Get(ap => ap.JobPostId == jobId && (status != null ? ap.Status == (int?)status : true));
      return query;
   }
   
   public IQueryable<DataTier.Entities.AppliedJob> GetAppliedJobsExceptPending(int jobId)
   {
      IQueryable<DataTier.Entities.AppliedJob> query = _appliedJobRepository.Get(ap => ap.JobPostId == jobId && 
         (ap.Status != (int?)AppliedJobEnum.AppliedJobStatus.Pending 
          && ap.Status != (int?)AppliedJobEnum.AppliedJobStatus.Reject));
      return query;
   }
   

   public IQueryable<DataTier.Entities.AppliedJob> GetAllAppliedJobs(int landownerId, AppliedJobEnum.AppliedJobStatus? status = null)
   {
      IQueryable<DataTier.Entities.AppliedJob> query = _appliedJobRepository.Get(aj =>
         aj.JobPost.PublishedBy == landownerId && (status != null ? aj.Status == (int?)status : true));
      return query;
   }

   public IQueryable<DataTier.Entities.AppliedJob> GetAppliedJobsFarmer(int farmerId, AppliedJobEnum.AppliedJobStatus? status = null)
   {
      IQueryable<DataTier.Entities.AppliedJob> query = _appliedJobRepository.Get(ap => ap.AppliedBy == farmerId && (status == null || ap.Status == (int?)status));
      return query;
   }

   public async Task<DataTier.Entities.AppliedJob> GetByJobAndAccount(int jobPostId, int accountId)
   {
      DataTier.Entities.AppliedJob appliedJob = 
         await _appliedJobRepository.GetFirstOrDefaultAsync(a => a.JobPostId == jobPostId && a.AppliedBy == accountId);
      if (appliedJob == null)
      {
         throw new Exception("Do not Applied Job");
      }

      return appliedJob;
   }

   public async Task<DataTier.Entities.AppliedJob> ApplyJob(int jobId, int appliedBy)
   {
      DataTier.Entities.AppliedJob checkExisted =
        (await _appliedJobRepository.GetFirstOrDefaultAsync(ap => ap.JobPostId == jobId && ap.AppliedBy == appliedBy))!;
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
      DataTier.Entities.AppliedJob? appliedJob = await _appliedJobRepository.GetFirstOrDefaultAsync(aj => aj.JobPostId == jobId && aj.AppliedBy == appliedBy);

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
      DataTier.Entities.AppliedJob? appliedJob = await _appliedJobRepository.GetByIdAsync(appliedJobId);
      
      if (appliedJob == null)
      {
         throw new MException(StatusCodes.Status400BadRequest, "Applied Job is not existed.");
      }
      appliedJob.Status = (int)AppliedJobEnum.AppliedJobStatus.Approve;
      appliedJob.ApprovedDate = DateTime.Now;
      _appliedJobRepository.Modify(appliedJob);
      if (appliedJob.JobPost.Type.Equals("PayPerTaskJob"))
      {
         IQueryable<DataTier.Entities.AppliedJob> rejectList = _appliedJobRepository.Get(a =>
            a.JobPostId == appliedJob.JobPostId
            && a.AppliedBy != appliedJob.AppliedBy);
         List<DataTier.Entities.AppliedJob> listAppliedJob = await rejectList.ToListAsync();
         listAppliedJob.ForEach(la =>
         {
            la.Status = (int)AppliedJobEnum.AppliedJobStatus.Reject;
         });
      }
      await _contextFactory.SaveAllAsync();

      return appliedJob;
   }

   public async Task<DataTier.Entities.AppliedJob> RejectJob(int appliedJobId)
   {
      DataTier.Entities.AppliedJob? appliedJob = await _appliedJobRepository.GetByIdAsync(appliedJobId);
      
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