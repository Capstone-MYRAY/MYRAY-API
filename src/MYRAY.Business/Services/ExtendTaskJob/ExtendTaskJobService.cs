using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Account;
using MYRAY.Business.DTOs.ExtendTaskJob;
using MYRAY.Business.DTOs.JobPost;
using MYRAY.Business.Enums;
using MYRAY.Business.Helpers;
using MYRAY.Business.Helpers.Paging;
using MYRAY.Business.Repositories.ExtendTaskJob;
using MYRAY.Business.Services.Account;
using MYRAY.Business.Services.JobPost;
using MYRAY.Business.Services.Notification;

namespace MYRAY.Business.Services.ExtendTaskJob;

public class ExtendTaskJobService : IExtendTaskJobService
{
    private readonly IMapper _mapper;
    private readonly IExtendTaskJobRepository _extendTaskJobRepository;
    private readonly IJobPostService _jobPostService;
    private readonly IAccountService _accountService;
    public ExtendTaskJobService(IMapper mapper, 
        IExtendTaskJobRepository extendTaskJobRepository,
        IAccountService accountService,
        IJobPostService jobPostService)
    {
        _mapper = mapper;
        _accountService = accountService;
        _jobPostService = jobPostService;
        _extendTaskJobRepository = extendTaskJobRepository;
    }


    public async Task<ExtendTaskJobDetail> CheckOneExtend(int jobPostId)
    {
       DataTier.Entities.ExtendTaskJob? query = await _extendTaskJobRepository.GetExtendTaskJobs(jobPostId).FirstOrDefaultAsync();

       if (query == null) return null;

       return _mapper.Map<ExtendTaskJobDetail>(query);
    }

    public ResponseDto.CollectiveResponse<ExtendTaskJobDetail> GetExtendTaskJobsAll(SearchExtendRequest searchExtendRequest, PagingDto pagingDto,
        SortingDto<ExtendTaskJobEnum.SortCriteriaExtendTaskJob> sortingDto,
        int? accountId = null)
    {
        IQueryable<DataTier.Entities.ExtendTaskJob> query = _extendTaskJobRepository.GetExtendTaskJobsAll(accountId);

        query = query.GetWithSearch(searchExtendRequest);

        query = query.GetWithSorting(sortingDto.SortColumn.ToString(), sortingDto.OrderBy);

        var result = query.GetWithPaging<ExtendTaskJobDetail, DataTier.Entities.ExtendTaskJob>(pagingDto, _mapper);

        return result;
    }

    public async Task<ExtendTaskJobDetail> CreateExtendTaskJob(CreateExtendRequest extendRequest, int requestBy)
    {
        DataTier.Entities.ExtendTaskJob extendTaskJob = _mapper.Map<DataTier.Entities.ExtendTaskJob>(extendRequest);
        extendTaskJob.RequestBy = requestBy;
        extendTaskJob.Status = (int?)ExtendTaskJobEnum.ExtendTaskJobStatus.Pending;
        extendTaskJob = await _extendTaskJobRepository.CreateExtendTaskJob(extendTaskJob);
        ExtendTaskJobDetail result = _mapper.Map<ExtendTaskJobDetail>(extendTaskJob);
        JobPostDetail jobPost = await _jobPostService.GetJobPostById(((int)extendTaskJob.JobPostId));
        GetAccountDetail accountDetail = await _accountService.GetAccountByIdAsync(extendTaskJob.RequestBy);
        // // Sent noti
        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {"type", "extendRequest"}
        };
        await PushNotification.SendMessage(jobPost.PublishedBy.ToString()
            , $"Yêu cầu gia hạn", $"{accountDetail.Fullname} đã yêu cầu gia hạn công việc {jobPost.Title}", data);

        
        return result;
    }

    public async Task<ExtendTaskJobDetail> UpdateExtendTaskJob(UpdateExtendRequest extendRequest, int requestBy)
    {
        DataTier.Entities.ExtendTaskJob extendTaskJob = _mapper.Map<DataTier.Entities.ExtendTaskJob>(extendRequest);
        extendTaskJob.RequestBy = requestBy;
        extendTaskJob = await _extendTaskJobRepository.UpdateExtendTaskJob(extendTaskJob);
        ExtendTaskJobDetail result = _mapper.Map<ExtendTaskJobDetail>(extendTaskJob);
        JobPostDetail jobPost = await _jobPostService.GetJobPostById(((int)extendTaskJob.JobPostId));
        GetAccountDetail accountDetail = await _accountService.GetAccountByIdAsync(extendTaskJob.RequestBy);
        // // Sent noti
        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {"type", "extendRequest"}
        };
        await PushNotification.SendMessage(jobPost.PublishedBy.ToString()
            , $"Thay đổi yêu cầu gia hạn", $"{accountDetail.Fullname} đã thay đổi yêu cầu gia hạn công việc {jobPost.Title}", data);

        
        return result;
    }

    public async Task<ExtendTaskJobDetail> DeleteExtendTaskJob(int id)
    {
        DataTier.Entities.ExtendTaskJob extendTaskJob = await _extendTaskJobRepository.DeleteExtendTaskJob(id);
        ExtendTaskJobDetail result = _mapper.Map<ExtendTaskJobDetail>(extendTaskJob);
        return result;
    }

    public async Task<ExtendTaskJobDetail> ApproveExtendTaskJob(int eTjId, int approvedBy)
    {
        DataTier.Entities.ExtendTaskJob extendTaskJob = await _extendTaskJobRepository.ApproveExtendTaskJobById(eTjId, approvedBy);
        ExtendTaskJobDetail result = _mapper.Map<ExtendTaskJobDetail>(extendTaskJob);
        return result;
    }

    public async Task<ExtendTaskJobDetail> RejectExtendTaskJob(int eTjId, int approvedBy)
    {
        DataTier.Entities.ExtendTaskJob extendTaskJob = await _extendTaskJobRepository.RejectExtendTaskJobById(eTjId, approvedBy);
        ExtendTaskJobDetail result = _mapper.Map<ExtendTaskJobDetail>(extendTaskJob);
        return result;
    }
}