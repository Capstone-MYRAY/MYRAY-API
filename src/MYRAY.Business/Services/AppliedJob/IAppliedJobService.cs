using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Account;
using MYRAY.Business.DTOs.JobPost;
using MYRAY.Business.Enums;

namespace MYRAY.Business.Services.AppliedJob;

public interface IAppliedJobService
{
    public ResponseDto.CollectiveResponse<AppliedJobDetail> GetAccountsApplied(
        PagingDto pagingDto,
        SortingDto<AppliedJobEnum.SortCriteriaAppliedJob> sortingDto,
        int jobPostId, AppliedJobEnum.AppliedJobStatus? status = null);
    
    public ResponseDto.CollectiveResponse<AppliedJobDetail> GetAllAccountsApplied(
        PagingDto pagingDto,
        SortingDto<AppliedJobEnum.SortCriteriaAppliedJob> sortingDto,
        int landownerId,
        AppliedJobEnum.AppliedJobStatus? status = null);

    public ResponseDto.CollectiveResponse<AppliedJobDetail> GetAccountsAppliedFarmer(
        PagingDto pagingDto,
        SortingDto<AppliedJobEnum.SortCriteriaAppliedJob> sortingDto,
        int farmerId, 
        AppliedJobEnum.AppliedJobStatus? status = null, int? startWork = null);

    public Task<AppliedJobDetail> ApplyJob(int jobId, int appliedBy);
    public Task<AppliedJobDetail> CancelApply(int jobId, int appliedBy);
    public Task<AppliedJobDetail> ApproveJob(int appliedJobId);
    public Task<AppliedJobDetail> RejectJob(int appliedJobId);

    public Task<AppliedJobDetail?> CheckApplied(int jobPostId, int appliedId);

    public Task<bool> CheckAppliedHourJob(int farmerId);

}