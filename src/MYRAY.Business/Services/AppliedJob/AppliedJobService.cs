using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.JobPost;
using MYRAY.Business.Enums;
using MYRAY.Business.Exceptions;
using MYRAY.Business.Helpers.Paging;
using MYRAY.Business.Repositories.Account;
using MYRAY.Business.Repositories.AppliedJob;
using MYRAY.Business.Repositories.JobPost;
using MYRAY.Business.Services.Notification;

namespace MYRAY.Business.Services.AppliedJob;

public class AppliedJobService : IAppliedJobService
{
    private readonly IMapper _mapper;
    private readonly IAppliedJobRepository _appliedJobRepository;
    private readonly IJobPostRepository _jobPostRepository;
    private readonly IAccountRepository _accountRepository;

    public AppliedJobService(IMapper mapper,
        IAppliedJobRepository appliedJobRepository,
        IAccountRepository accountRepository,
        IJobPostRepository jobPostRepository)
    {
        _mapper = mapper;
        _appliedJobRepository = appliedJobRepository;
        _jobPostRepository = jobPostRepository;
        _accountRepository = accountRepository;
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

    public async Task<AppliedJobDetail> ApplyJob(int jobId, int appliedBy)
    {
        DataTier.Entities.JobPost jobPost = await _jobPostRepository.GetJobPostById(jobId);
        if (jobPost.Status == (int?)JobPostEnum.JobPostStatus.Enough)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Job post enough farmer");
        }

        var applied = await _appliedJobRepository.ApplyJob(jobId, appliedBy);
        // Sent noti
        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            { "type", "appliedFarmer" }
        };
        DataTier.Entities.Account applyBy = await _accountRepository.GetAccountByIdAsync(applied.AppliedBy);
        await PushNotification.SendMessage(jobPost.PublishedBy.ToString()!
            , $"Yêu cầu ứng tuyển", $"{applyBy.Fullname} đã ứng tuyển vào công việc {jobPost.Title}", data);
        var result = _mapper.Map<AppliedJobDetail>(applied);
        return result;
    }

    public async Task<AppliedJobDetail> CancelApply(int jobId, int appliedBy)
    {
        var result1 = await _appliedJobRepository.CancelApply(jobId, appliedBy);
        var result = _mapper.Map<AppliedJobDetail>(result1);
        return result;
    }

    public async Task<AppliedJobDetail> ApproveJob(int appliedJobId)
    {
        var result = await _appliedJobRepository.ApproveJob(appliedJobId);
        try
        {
            // Sent noti
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "type", "appliedFarmer" }
            };
            await PushNotification.SendMessage(result.AppliedBy.ToString()
                , $"Ứng tuyển thành công",
                $"{result.AppliedByNavigation.Fullname} đã được nhận vào công việc {result.JobPost.Title}", data);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        var result1 = _mapper.Map<AppliedJobDetail>(result);
        return result1;
    }

    public async Task<AppliedJobDetail> RejectJob(int appliedJobId)
    {
        var result = await _appliedJobRepository.RejectJob(appliedJobId);

        // // Sent noti
        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            { "type", "appliedFarmer" }
        };
        await PushNotification.SendMessage(result.AppliedBy.ToString()
            , $"Ứng tuyển không thành công",
            $"{result.AppliedByNavigation.Fullname} đã bị từ chối nhận vào công việc {result.JobPost.Title}", data);

        var result1 = _mapper.Map<AppliedJobDetail>(result);
        return result1;
    }

    public async Task<int> AlreadyApplied(int accountId)
    {
        IQueryable<DataTier.Entities.AppliedJob> query = _appliedJobRepository.GetAppliedJobsFarmer(accountId)
            .Where(aj =>
                aj.Status == (int?)AppliedJobEnum.AppliedJobStatus.Pending
                || aj.Status == (int?)AppliedJobEnum.AppliedJobStatus.Approve);

        var result = await query.CountAsync();

        return result;
    }

    public async Task<AppliedJobDetail?> CheckApplied(int jobPostId, int appliedId)
    {
        var applied = await _appliedJobRepository.CheckApplied(jobPostId, appliedId);
        var result = _mapper.Map<AppliedJobDetail>(applied);
        return result;
    }

    public async Task<bool> CheckAppliedHourJob(int farmerId)
    {
        return await _appliedJobRepository.CheckAppliedHourJob(farmerId);
    }

    public async Task<int> CountAppliedJob(int jobPostId)
    {
        IQueryable<DataTier.Entities.AppliedJob> query = _appliedJobRepository.GetAllAppliedJobs(jobPostId)
            .Where(a => a.Status == (int?)AppliedJobEnum.AppliedJobStatus.Approve 
                        || a.Status == (int?)AppliedJobEnum.AppliedJobStatus.End);
        var result = await query.CountAsync();
        return result;
    }
}