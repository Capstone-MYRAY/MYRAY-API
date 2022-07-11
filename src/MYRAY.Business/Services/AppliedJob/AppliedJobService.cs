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


    public ResponseDto.CollectiveResponse<AppliedJobDetail> GetAccountsApplied(
        PagingDto pagingDto, 
        SortingDto<AppliedJobEnum.SortCriteriaAppliedJob> sortingDto,
        int jobPostId, AppliedJobEnum.AppliedJobStatus? status = null)
    {
        IQueryable<DataTier.Entities.AppliedJob> query = _appliedJobRepository.GetAppliedJobs(jobPostId, status);

        query = query.GetWithSorting(sortingDto.SortColumn.ToString(), sortingDto.OrderBy);
        
        var result = query.GetWithPaging<AppliedJobDetail, DataTier.Entities.AppliedJob>(pagingDto, _mapper);

        return result;
    }

    public ResponseDto.CollectiveResponse<AppliedJobDetail> GetAllAccountsApplied(
        PagingDto pagingDto, 
        SortingDto<AppliedJobEnum.SortCriteriaAppliedJob> sortingDto,
        int landownerId, 
        AppliedJobEnum.AppliedJobStatus? status = null)
    {
        IQueryable<DataTier.Entities.AppliedJob> query = _appliedJobRepository.GetAllAppliedJobs(landownerId, status);

        query = query.GetWithSorting(sortingDto.SortColumn.ToString(), sortingDto.OrderBy);
        
        var result = query.GetWithPaging<AppliedJobDetail, DataTier.Entities.AppliedJob>(pagingDto, _mapper);

        return result;
    }

    public ResponseDto.CollectiveResponse<AppliedJobDetail> GetAccountsAppliedFarmer(
        PagingDto pagingDto, 
        SortingDto<AppliedJobEnum.SortCriteriaAppliedJob> sortingDto,
        int farmerId, 
        AppliedJobEnum.AppliedJobStatus? status = null,
        int? statusWork = null)
    {
        IQueryable<DataTier.Entities.AppliedJob> query = _appliedJobRepository.GetAppliedJobsFarmer(farmerId, status);
        if (statusWork != null)
        {
            query = query.Where(a => a.JobPost.StatusWork == statusWork);
        }
        query = query.GetWithSorting(sortingDto.SortColumn.ToString(), sortingDto.OrderBy);
        
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

    public async Task<DataTier.Entities.AppliedJob?> CheckApplied(int jobPostId, int appliedId)
    {
        return await _appliedJobRepository.CheckApplied(jobPostId, appliedId);
    }

    public async Task<bool> CheckAppliedHourJob(int farmerId)
    {
        return await _appliedJobRepository.CheckAppliedHourJob(farmerId);
    }
}