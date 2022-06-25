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
using MYRAY.Business.Repositories.PostType;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Services.JobPost;

public class JobPostService : IJobPostService
{
    private readonly IMapper _mapper;
    private readonly IJobPostRepository _jobPostRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IPostTypeRepository _postTypeRepository;
    private readonly IConfiguration _configuration;

    public JobPostService(IMapper mapper, 
        IJobPostRepository jobPostRepository,
        IAccountRepository accountRepository,
        IPostTypeRepository postTypeRepository,
        IConfiguration configuration)
    {
        _mapper = mapper;
        _jobPostRepository = jobPostRepository;
        _configuration = configuration;
        _accountRepository = accountRepository;
        _postTypeRepository = postTypeRepository;
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
                    // PostTypeId = (int)jobPost.PostTypeId,
                    PinDate1 = (DateTime)startPin,
                    Status = 0
                });
                startPin = startPin.Value.AddDays(1);
            }
        }
        
        DataTier.Entities.JobPost newJobPost = _mapper.Map<DataTier.Entities.JobPost>(jobPost);
        newJobPost.CreatedDate = DateTime.Now;
        newJobPost.PublishedBy = publishedBy;
        if (newJobPost.PayPerHourJob != null) 
            newJobPost.Type = "PayPerHourJob";
        else 
            newJobPost.Type = "PayPerTaskJob";
        newJobPost.Status = (int?)JobPostEnum.JobPostStatus.Pending;
        
        
        //--Get Setting From Json File
        float priceJobPost = float.Parse(_configuration.GetSection("Money").GetSection("JobPost").Value);
        float pricePoint = float.Parse(_configuration.GetSection("Money").GetSection("Point").Value);
        float earnPoint = float.Parse(_configuration.GetSection("Money").GetSection("EarnPoint").Value);

        //--Get more information to calculate
        DataTier.Entities.PostType postType = await _postTypeRepository.GetPostTypeById((int)jobPost.PostTypeId);
        DataTier.Entities.Account accountPost = await _accountRepository.GetAccountByIdAsync(publishedBy);
        //--Calculate Price
        float aPriceJobPost = (float)(priceJobPost * newJobPost.NumPublishDay);
        float aPricePinPost = (float)(postType.Price * jobPost.NumberPinDay);
        float aPriceUsePoint = 0;
        //-- Check use point
        if (jobPost.UsePoint != null || jobPost.UsePoint != 0)
        {
            aPriceUsePoint = (float)(jobPost.UsePoint * pricePoint);
        }
        //-- New payment
        PaymentHistory newPayment = new PaymentHistory
        {
            JobPostId = newJobPost.Id,
            CreatedBy = publishedBy,
            Status = (int?)PaymentHistoryEnum.PaymentHistoryStatus.Pending,
            CreatedDate = DateTime.Now,
            ActualPrice = aPriceJobPost + aPricePinPost,
            BalanceFluctuation =  - (aPricePinPost + aPriceJobPost - aPriceUsePoint),
            Balance = accountPost.Balance - (aPricePinPost + aPriceJobPost - aPriceUsePoint),
            EarnedPoint = (int?)((aPriceJobPost + aPricePinPost) / earnPoint),
            UsedPoint = jobPost.UsePoint != null ? jobPost.UsePoint : 0,
            BelongedId = publishedBy,
            Message = "Tạo bài đăng mới #" + newJobPost.Id,
            JobPostPrice = priceJobPost,
            PointPrice = (int?)pricePoint
        };

        newJobPost = await _jobPostRepository.CreateJobPost(newJobPost, newJobPost.PayPerHourJob, newJobPost.PayPerTaskJob, listPin, newPayment);
        //-- Done Insert Job Post

        
        
        var result = _mapper.Map<JobPostDetail>(newJobPost);
        return result;
    }

    public async Task<JobPostDetail> UpdateJobPost(UpdateJobPost jobPost, int publishedBy)
    {
        //-- Delete PinDate List
        IQueryable<PinDate> queryPin = (IQueryable<PinDate>)_jobPostRepository.GetPinDateByJobPost(jobPost.Id);
        ICollection<PinDate> list = queryPin.ToList();
        _jobPostRepository.DeletePinDate(list);
        //-- Insert PinDate Again
        ICollection<PinDate> listPin = null!;
        if (jobPost.PinDate != null && jobPost.NumberPinDay != null)
        {
            DateTime? startPin = jobPost.PinDate;
            listPin = new List<PinDate>();
            for (int i = 0; i < jobPost.NumberPinDay; i++)
            {
                listPin.Add(new PinDate()
                {
                    // PostTypeId = (int)jobPost.PostTypeId,
                    PinDate1 = (DateTime)startPin,
                    Status = 0
                });
                startPin = startPin.Value.AddDays(1);
            }
        }
        //-- Map and restore value
        DataTier.Entities.JobPost updateJobPost = _mapper.Map<DataTier.Entities.JobPost>(jobPost);
        updateJobPost.UpdatedDate = DateTime.Now;
        updateJobPost.PublishedBy = publishedBy;
        if (updateJobPost.PayPerHourJob != null) 
            updateJobPost.Type = "PayPerHourJob";
        else 
            updateJobPost.Type = "PayPerTaskJob";
        updateJobPost.Status = (int?)JobPostEnum.JobPostStatus.Pending;

        //-- Add Payment
        
        //--Get Setting From Json File
        float priceJobPost = float.Parse(_configuration.GetSection("Money").GetSection("JobPost").Value);
        float pricePoint = float.Parse(_configuration.GetSection("Money").GetSection("Point").Value);
        float earnPoint = float.Parse(_configuration.GetSection("Money").GetSection("EarnPoint").Value);

        //--Get more information to calculate
        DataTier.Entities.PostType postType = await _postTypeRepository.GetPostTypeById((int)jobPost.PostTypeId);
        DataTier.Entities.Account accountPost = await _accountRepository.GetAccountByIdAsync(publishedBy);
        //--Calculate Price
        float aPriceJobPost = (float)(priceJobPost * updateJobPost.NumPublishDay);
        float aPricePinPost = (float)(postType.Price * jobPost.NumberPinDay);
        float aPriceUsePoint = 0;
        //-- Check use point
        if (jobPost.UsePoint != null || jobPost.UsePoint != 0)
        {
            aPriceUsePoint = (float)(jobPost.UsePoint * pricePoint);
        }
        //-- New payment
        PaymentHistory newPayment = new PaymentHistory
        {
            JobPostId = updateJobPost.Id,
            CreatedBy = publishedBy,
            Status = (int?)PaymentHistoryEnum.PaymentHistoryStatus.Pending,
            CreatedDate = DateTime.Now,
            ActualPrice = aPriceJobPost + aPricePinPost,
            BalanceFluctuation =  - (aPricePinPost + aPriceJobPost - aPriceUsePoint),
            Balance = accountPost.Balance - (aPricePinPost + aPriceJobPost - aPriceUsePoint),
            EarnedPoint = (int?)((aPriceJobPost + aPricePinPost) / earnPoint),
            UsedPoint = jobPost.UsePoint != null ? jobPost.UsePoint : 0,
            BelongedId = publishedBy,
            Message = "Tạo bài đăng mới #" + updateJobPost.Id,
            JobPostPrice = priceJobPost,
            PointPrice = (int?)pricePoint
        };
        //-- End Add Payment
        
        updateJobPost = await _jobPostRepository.UpdateJobPost(updateJobPost, updateJobPost.PayPerHourJob, updateJobPost.PayPerTaskJob, listPin, newPayment);
        
        var result = _mapper.Map<JobPostDetail>(updateJobPost);
        return result;
    }

    public async Task<JobPostDetail> DeleteJobPost(int jobPostId)
    {
        DataTier.Entities.JobPost deleteGuidepost = await _jobPostRepository.DeleteJobPost(jobPostId);
        var result = _mapper.Map<JobPostDetail>(deleteGuidepost);
        return result;
    }

    public async Task<JobPostDetail> CancelJobPost(int jobPostId)
    {
        DataTier.Entities.JobPost cancelJobPost = await _jobPostRepository.CancelJobPost(jobPostId);
        var result = _mapper.Map<JobPostDetail>(cancelJobPost);
        return result;
    }

    public async Task<JobPostDetail> ApproveJobPost(int jobPostId, int approvedBy)
    {
        var jobPost = await _jobPostRepository.ApproveJobPost(jobPostId, approvedBy);
        var result = _mapper.Map<JobPostDetail>(jobPost);
        return result;
    }

    public async Task<JobPostDetail> RejectJobPost(RejectJobPost rejectJobPost, int approvedBy)
    {
        var jobPost = await _jobPostRepository.RejectJobPost(rejectJobPost, approvedBy);
        var result = _mapper.Map<JobPostDetail>(jobPost);
        return result;
    }

    public async Task<JobPostDetail> StartJobPost(int jobPostId)
    {
        var jobPost = await _jobPostRepository.StartJob(jobPostId);
        var result = _mapper.Map<JobPostDetail>(jobPost);
        return result;
    }
}