using AutoMapper;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.JobPost;
using MYRAY.Business.Enums;
using MYRAY.Business.Helpers.Paging;
using MYRAY.Business.Repositories.AppliedJob;

namespace MYRAY.Business.Services.AppliedJob;

public class AppliedJobService : IAppliedJobService
{
    private readonly IMapper _mapper;
    private readonly IAppliedJobRepository _appliedJobRepository;

    public AppliedJobService(IMapper mapper, IAppliedJobRepository appliedJobRepository)
    {
        _mapper = mapper;
        _appliedJobRepository = appliedJobRepository;
    }


    public ResponseDto.CollectiveResponse<AppliedJobDetail> GetAccountsApplied(PagingDto pagingDto, int jobPostId, AppliedJobEnum.AppliedJobStatus? status = null)
    {
        IQueryable<DataTier.Entities.AppliedJob> query = _appliedJobRepository.GetAppliedJobs(jobPostId, status);

        var result = query.GetWithPaging<AppliedJobDetail, DataTier.Entities.AppliedJob>(pagingDto, _mapper);

        return result;
    }

    public async Task<DataTier.Entities.AppliedJob> ApplyJob(int jobId, int appliedBy)
    {
        var result = await _appliedJobRepository.ApplyJob(jobId, appliedBy);
        return result;
    }

    public async Task<DataTier.Entities.AppliedJob> CancelApply(int jobId, int appliedBy)
    {
        var result = await _appliedJobRepository.CancelApply(jobId, appliedBy);
        return result;
    }

    public async Task<DataTier.Entities.AppliedJob> ApproveJob(int appliedJobId)
    {
        var result = await _appliedJobRepository.ApproveJob(appliedJobId);
        return result;
    }

    public async Task<DataTier.Entities.AppliedJob> RejectJob(int appliedJobId)
    {
        var result = await _appliedJobRepository.RejectJob(appliedJobId);
        return result;
    }
}