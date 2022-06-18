using Microsoft.AspNetCore.Http;
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
}