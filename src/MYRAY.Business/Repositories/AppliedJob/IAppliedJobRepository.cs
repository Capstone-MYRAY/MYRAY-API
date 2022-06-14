using MYRAY.Business.Enums;

namespace MYRAY.Business.Repositories.AppliedJob;

public interface IAppliedJobRepository
{
    IQueryable<DataTier.Entities.AppliedJob> GetAppliedJobs(int jobId, AppliedJobEnum.AppliedJobStatus? status = null);
    Task<DataTier.Entities.AppliedJob> ApplyJob(int jobId, int appliedBy);
    Task<DataTier.Entities.AppliedJob> CancelApply(int jobId,  int appliedBy);

    Task<DataTier.Entities.AppliedJob> ApproveJob(int appliedJobId);
    Task<DataTier.Entities.AppliedJob> RejectJob(int appliedJobId);
    
}