using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Account;
using MYRAY.Business.DTOs.JobPost;
using MYRAY.Business.Enums;

namespace MYRAY.Business.Services.AppliedJob;

public interface IAppliedJobService
{
    public ResponseDto.CollectiveResponse<AppliedJobDetail> GetAccountsApplied(PagingDto pagingDto,
        int jobPostId, AppliedJobEnum.AppliedJobStatus? status = null);
    
    public ResponseDto.CollectiveResponse<AppliedJobDetail> GetAccountsAppliedFarmer(PagingDto pagingDto,
        int farmerId, AppliedJobEnum.AppliedJobStatus? status = null, int? startWork = null);

    public Task<DataTier.Entities.AppliedJob> ApplyJob(int jobId, int appliedBy);
    public Task<DataTier.Entities.AppliedJob> CancelApply(int jobId, int appliedBy);
    public Task<DataTier.Entities.AppliedJob> ApproveJob(int appliedJobId);
    public Task<DataTier.Entities.AppliedJob> RejectJob(int appliedJobId);

    public Task<DataTier.Entities.AppliedJob?> CheckApplied(int jobPostId, int appliedId);

}