using AutoMapper;
using Microsoft.Extensions.Configuration;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.JobPost;
using MYRAY.Business.Enums;
using MYRAY.Business.Helpers;
using MYRAY.Business.Helpers.Paging;
using MYRAY.Business.Repositories.Account;
using MYRAY.Business.Repositories.JobPost;
using MYRAY.Business.Repositories.PaymentHistory;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Services.JobPost;

public class JobPostService : IJobPostService
{
    private readonly IMapper _mapper;
    private readonly IJobPostRepository _jobPostRepository;
    private readonly IPaymentHistoryRepository _paymentHistoryRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IConfiguration _configuration;

    public JobPostService(IMapper mapper, 
        IJobPostRepository jobPostRepository,
        IPaymentHistoryRepository paymentHistoryRepository,
        IAccountRepository accountRepository,
        IConfiguration configuration)
    {
        _mapper = mapper;
        _jobPostRepository = jobPostRepository;
        _paymentHistoryRepository = paymentHistoryRepository;
        _configuration = configuration;
        _accountRepository = accountRepository;
    }

    public ResponseDto.CollectiveResponse<JobPostDetail> GetJobPosts(SearchJobPost searchJobPost, PagingDto pagingDto, SortingDto<JobPostEnum.JobPostSortCriteria> sortingDto, int? publishId = null)
    {
        List<DataTier.Entities.JobPost> listPin = null;
        IQueryable<DataTier.Entities.JobPost> query = _jobPostRepository.GetJobPosts(publishId);

        query = query.GetWithSearch(searchJobPost);
        
        if (publishId == null)
        {
            listPin =  _jobPostRepository.GetPinPost().ToList();
            var pinId = listPin.Select(x => x.Id);
            query = query.Where(p => !pinId.Contains(p.Id));
        }
        
        query = query.GetWithSorting(sortingDto.SortColumn.ToString(), sortingDto.OrderBy);
        
        var result = query.GetWithPaging<JobPostDetail, DataTier.Entities.JobPost>(pagingDto, _mapper);
        if (publishId == null)
        {
            var listP = _mapper.ProjectTo<JobPostDetail>(_jobPostRepository.GetPinPost());
            var listAfterAdd = result.ListObject.ToList();
            listAfterAdd.InsertRange(0, listP);
            result.ListObject = listAfterAdd;
            result.PagingMetadata!.PageSize += (result.PagingMetadata.PageSize < listAfterAdd.Count) ? 0 : listPin.Count;
        }
        return result;
    }

    public async Task<JobPostDetail> GetJobPostById(int id)
    {
        DataTier.Entities.JobPost jobPost = await _jobPostRepository.GetJobPostById(id);
        JobPostDetail result = _mapper.Map<JobPostDetail>(jobPost);
        return result;
    }

    public async Task<JobPostDetail> CreateJobPost(CreateJobPost jobPost, int publishedBy)
    {
        
        ICollection<PinDate> listPin = null!;
        if (jobPost.PinDate != null && jobPost.NumberPinDay != null)
        {
            DateTime? startPin = jobPost.PinDate;
            listPin = new List<PinDate>();
            for (int i = 0; i < jobPost.NumberPinDay; i++)
            {
                listPin.Add(new PinDate
                {
                    PostTypeId = (int)jobPost.PostTypeId,
                    PinDate1 = (DateTime)startPin,
                    Status = 0
                });
                startPin = startPin.Value.AddDays(1);
            }
        }

        var pn = listPin;
        DataTier.Entities.JobPost newJobPost = _mapper.Map<DataTier.Entities.JobPost>(jobPost);
        newJobPost.CreatedDate = DateTime.Now;
        newJobPost.PublishedBy = publishedBy;
        if (newJobPost.PayPerHourJob != null) 
            newJobPost.Type = "PayPerHourJob";
        else 
            newJobPost.Type = "PayPerTaskJob";
        newJobPost.Status = (int?)JobPostEnum.JobPostStatus.Pending;
        
        newJobPost = await _jobPostRepository.CreateJobPost(newJobPost, newJobPost.PayPerHourJob, newJobPost.PayPerTaskJob, listPin);
        float priceJobPost = float.Parse(_configuration.GetSection("Money").GetSection("JobPost").Value);
        float pricePoint = float.Parse(_configuration.GetSection("Money").GetSection("Point").Value);

        float aPriceJobPost = (float)(pricePoint * newJobPost.NumPublishDay);
        
        PaymentHistory newPayment = new PaymentHistory
        {
            JobPostId = newJobPost.Id,
            CreatedBy = newJobPost.PublishedBy,
            Status = (int?)PaymentHistoryEnum.PaymentHistoryStatus.Pending,
            CreatedDate = DateTime.Now,
            
        };
        
        
        var result = _mapper.Map<JobPostDetail>(newJobPost);
        return result;
    }

    public async Task<JobPostDetail> UpdateJobPost(UpdateJobPost jobPost)
    {
        IQueryable<PinDate> queryPin = (IQueryable<PinDate>)_jobPostRepository.GetPinDateByJobPost(jobPost.Id);
        ICollection<PinDate> list = queryPin.ToList();
        _jobPostRepository.DeletePinDate(list);
        
        ICollection<PinDate> listPin = null!;
        if (jobPost.PinDate != null && jobPost.NumberPinDay != null)
        {
            DateTime? startPin = jobPost.PinDate;
            listPin = new List<PinDate>();
            for (int i = 0; i < jobPost.NumberPinDay; i++)
            {
                listPin.Add(new PinDate()
                {
                    PostTypeId = (int)jobPost.PostTypeId,
                    PinDate1 = (DateTime)startPin,
                    Status = 0
                });
                startPin = startPin.Value.AddDays(1);
            }
        }
        
        DataTier.Entities.JobPost updateJobPost = _mapper.Map<DataTier.Entities.JobPost>(jobPost);
        updateJobPost.UpdatedDate = DateTime.Now;
        if (updateJobPost.PayPerHourJob != null) 
            updateJobPost.Type = "PayPerHourJob";
        else 
            updateJobPost.Type = "PayPerTaskJob";
        updateJobPost.Status = (int?)JobPostEnum.JobPostStatus.Pending;

        updateJobPost = await _jobPostRepository.UpdateJobPost(updateJobPost, updateJobPost.PayPerHourJob, updateJobPost.PayPerTaskJob, listPin);
        
        var result = _mapper.Map<JobPostDetail>(updateJobPost);
        return result;
    }

    public async Task<JobPostDetail> DeleteJobPost(int jobPostId)
    {
        DataTier.Entities.JobPost deleteGuidepost = await _jobPostRepository.DeleteJobPost(jobPostId);
        var result = _mapper.Map<JobPostDetail>(deleteGuidepost);
        return result;
    }
}