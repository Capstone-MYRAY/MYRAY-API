using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.JobPost;
using MYRAY.Business.Enums;
using MYRAY.Business.Exceptions;
using MYRAY.Business.Helpers.Paging;
using MYRAY.Business.Repositories.Account;
using MYRAY.Business.Repositories.Config;
using MYRAY.Business.Repositories.JobPost;
using MYRAY.Business.Repositories.PostType;
using MYRAY.Business.Repositories.TreeJob;
using MYRAY.Business.Repositories.TreeType;
using MYRAY.Business.Services.Notification;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Services.JobPost;

public class JobPostService : IJobPostService
{
    private readonly IMapper _mapper;
    private readonly IJobPostRepository _jobPostRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IPostTypeRepository _postTypeRepository;
    private readonly ITreeJobRepository _treeJobRepository;
    private readonly ITreeTypeRepository _treeTypeRepository;
    private readonly IConfiguration _configuration;
    private readonly IConfigRepository _configRepository;

    public JobPostService(IMapper mapper,
        IJobPostRepository jobPostRepository,
        IAccountRepository accountRepository,
        IPostTypeRepository postTypeRepository,
        ITreeJobRepository treeJobRepository,
        ITreeTypeRepository treeTypeRepository,
        IConfigRepository configRepository,
        IConfiguration configuration)
    {
        _mapper = mapper;
        _jobPostRepository = jobPostRepository;
        _configuration = configuration;
        _accountRepository = accountRepository;
        _postTypeRepository = postTypeRepository;
        _treeJobRepository = treeJobRepository;
        _configRepository = configRepository;
        _treeTypeRepository = treeTypeRepository;
    }

    public ResponseDto.CollectiveResponse<JobPostDetail> GetJobPosts(
        SearchJobPost searchJobPost,
        PagingDto pagingDto,
        SortingDto<JobPostEnum.JobPostSortCriteria> sortingDto,
        int? publishId = null,
        bool isFarmer = false)
    {
        List<DataTier.Entities.JobPost> listPin = null;
        IQueryable<DataTier.Entities.JobPost> query = _jobPostRepository.GetJobPosts(publishId);
        if (isFarmer)
        {
            query = query.Where(post => post.Status == (int?)JobPostEnum.JobPostStatus.Posted
                                        || post.Status == (int?)JobPostEnum.JobPostStatus.Expired);
        }

        #region FilterJobPost

        // query = query.GetWithSearch(searchJobPost);
        if (searchJobPost.Title.Length != 0)
        {
            query = query.Where(j => j.Title.ToLower().Contains(searchJobPost.Title.ToLower()));
        }

        if (searchJobPost.Status != null)
        {
            query = query.Where(j => j.Status == searchJobPost.Status);
        }

        if (searchJobPost.StatusWork != null)
        {
            query = query.Where(j => j.StatusWork == searchJobPost.StatusWork);
        }

        if (searchJobPost.Type.Length != 0)
        {
            query = query.Where(j => j.Type.ToLower().Equals(searchJobPost.Type.ToLower()));
        }

        if (searchJobPost.GardenId != null)
        {
            query = query.Where(j => j.GardenId == searchJobPost.GardenId);
        }

        if (searchJobPost.IsNotEndWork is true)
        {
            query = query.Where(j => j.StatusWork != (int?)JobPostEnum.JobPostWorkStatus.Done);
        }

        if (searchJobPost.Province != null)
        {
            query = query.Where(j => j.Garden.Area.Province!.ToLower().Contains(searchJobPost.Province.ToLower()));
        }

        if (searchJobPost.District != null)
        {
            query = query.Where(j => j.Garden.Area.District!.ToLower().Contains(searchJobPost.District.ToLower()));
        }

        if (searchJobPost.Commune != null)
        {
            query = query.Where(j => j.Garden.Area.Commune!.ToLower().Contains(searchJobPost.Commune.ToLower()));
        }

        if (searchJobPost.WorkTypeId != null)
        {
            query = query.Where(j => j.WorkTypeId == searchJobPost.WorkTypeId);
        }

        if (searchJobPost.StartDateFrom != null)
        {
            query = query.Where(j =>
                j.StartJobDate == null || j.StartJobDate.Value.Date >= searchJobPost.StartDateFrom.Value.Date);
        }

        if (searchJobPost.StartDateTo != null)
        {
            query = query.Where(j => j.StartJobDate == null || j.StartJobDate.Value.Date <= searchJobPost.StartDateTo);
        }

        if (searchJobPost.SalaryFrom != null)
        {
            query = query.Where(j =>
                j.PayPerHourJob.Salary >= searchJobPost.SalaryFrom
                || j.PayPerTaskJob.Salary >= searchJobPost.SalaryFrom);
        }

        if (searchJobPost.SalaryTo != null)
        {
            query = query.Where(j =>
                j.PayPerHourJob.Salary <= searchJobPost.SalaryTo
                || j.PayPerTaskJob.Salary <= searchJobPost.SalaryTo);
        }


        if (searchJobPost.TreeType != null)
        {
            string[] treeTypeList = searchJobPost.TreeType.Trim().Split(",");
            int[] treeTypeId = treeTypeList.Select(int.Parse).ToArray();
            var jobPostId =
                _treeJobRepository
                    .GetTreeJobs()
                    .Where(tt => treeTypeId.Contains(tt.TreeTypeId))
                    .Select(tj => tj.JobPostId)
                    .ToList();
            query = query.Where(j => jobPostId.Contains(j.Id));
        }


        List<DataTier.Entities.JobPost> test = query.ToList();

        if (isFarmer)
        {
            listPin = _jobPostRepository.GetPinPost().ToList();
            var pinId = listPin.Select(x => x.Id);
            query = query.Where(p => !pinId.Contains(p.Id));
        }

        #endregion

        query = query.GetWithSorting(sortingDto.SortColumn.ToString(), sortingDto.OrderBy);

        var result = query.GetWithPaging<JobPostDetail, DataTier.Entities.JobPost>(pagingDto, _mapper);

        if (isFarmer)
        {
            var listP = _mapper.ProjectTo<JobPostDetail>(_jobPostRepository.GetPinPost());
            result.SecondObject = listP.ToList();
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
        // Create pin date
        ICollection<PinDate> listPin = null!;
        if (jobPost.PinDate != null && jobPost.NumberPinDay != null)
        {
            DateTime? startPin = jobPost.PinDate;
            listPin = new List<PinDate>();
            for (int i = 0; i < jobPost.NumberPinDay; i++)
            {
                listPin.Add(new PinDate
                {
                    PinDate1 = (DateTime)startPin,
                    Status = 0
                });
                startPin = startPin.Value.AddDays(1);
            }
        }

        DataTier.Entities.JobPost newJobPost = _mapper.Map<DataTier.Entities.JobPost>(jobPost);
        newJobPost.CreatedDate = DateTime.Now;
        newJobPost.PublishedBy = publishedBy;
        newJobPost.PostTypeId = jobPost.PostTypeId;
        if (newJobPost.PayPerHourJob != null)
            newJobPost.Type = "PayPerHourJob";
        else
            newJobPost.Type = "PayPerTaskJob";
        newJobPost.Status = (int?)JobPostEnum.JobPostStatus.Pending;


        //--Get Setting From Db Config
        float pricePoint = float.Parse(_configRepository.GetConfigByKey("Point")!);
        float earnPoint = float.Parse(_configRepository.GetConfigByKey("EarnPoint")!);

        //--Get more information to calculate
        DataTier.Entities.Account accountPost = await _accountRepository.GetAccountByIdAsync(publishedBy);
        float aPricePinPost = 0;
        double? postTypePrice = null;
        if (jobPost.PostTypeId != null)
        {
            DataTier.Entities.PostType postType = await _postTypeRepository.GetPostTypeById((int)jobPost.PostTypeId);
            newJobPost.TotalPinDay = jobPost.NumberPinDay;
            newJobPost.StartPinDate = jobPost.PinDate;
            postTypePrice = postType.Price;
            if (postType == null)
            {
                throw new Exception("Post Type is not existed");
            }

            aPricePinPost = (float)(postType.Price * jobPost.NumberPinDay);
        }

        //--Calculate Price
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
            ActualPrice = aPricePinPost,
            BalanceFluctuation = -(aPricePinPost - aPriceUsePoint),
            Balance = accountPost.Balance - (aPricePinPost - aPriceUsePoint),
            EarnedPoint = (int?)Math.Round((aPricePinPost - aPriceUsePoint) / earnPoint),
            UsedPoint = jobPost.UsePoint ?? 0,
            BelongedId = publishedBy,
            Message = "Tạo bài đăng mới #" + newJobPost.Id,
            TotalPinDay = jobPost.NumberPinDay,
            PostTypePrice = (int?)postTypePrice,
            PointPrice = (int?)pricePoint
        };
        if (newPayment.Balance < 0)
        {
            throw new Exception("You not enough money to post job");
        }

        if (accountPost.Point < newPayment.UsedPoint)
        {
            throw new Exception("You not enough point to post job");
        }

        newJobPost = await _jobPostRepository.CreateJobPost(newJobPost, newJobPost.PayPerHourJob,
            newJobPost.PayPerTaskJob, listPin, newPayment);
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
        _treeJobRepository.DeleteTreeJob(jobPost.Id);
        if (jobPost.TreeJobs != null)
            _treeJobRepository.InsertTreeJob(jobPost.TreeJobs, jobPost.Id);
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
        float pricePoint = float.Parse((_configRepository.GetConfigByKey("Point"))!);
        float earnPoint = float.Parse((_configRepository.GetConfigByKey("EarnPoint"))!);

        //--Get more information to calculate
        DataTier.Entities.Account accountPost = await _accountRepository.GetAccountByIdAsync(publishedBy);

        float aPricePinPost = 0;
        double? postTypePrice = null;
        if (jobPost.PostTypeId != null)
        {
            updateJobPost.TotalPinDay = jobPost.NumberPinDay;
            updateJobPost.StartPinDate = jobPost.PinDate;
            DataTier.Entities.PostType postType = await _postTypeRepository.GetPostTypeById((int)jobPost.PostTypeId);
            postTypePrice = postType.Price;
            if (postType == null)
            {
                throw new Exception("Post Type is not existed");
            }

            aPricePinPost = (float)(postType.Price * jobPost.NumberPinDay);
        }

        //--Calculate Price
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
            ActualPrice = aPricePinPost,
            BalanceFluctuation = -(aPricePinPost - aPriceUsePoint),
            Balance = accountPost.Balance - (aPricePinPost - aPriceUsePoint),
            EarnedPoint = (int?)((aPricePinPost - aPriceUsePoint) / earnPoint),
            UsedPoint = jobPost.UsePoint ?? 0,
            BelongedId = publishedBy,
            Message = "Tạo bài đăng mới #" + updateJobPost.Id,
            TotalPinDay = jobPost.NumberPinDay,
            // NumberPublishedDay = jobPost.NumPublishDay,
            PostTypePrice = (int?)postTypePrice,
            PointPrice = (int?)pricePoint
        };
        //-- End Add Payment
        if (newPayment.Balance < 0)
        {
            throw new Exception("You not enough money to post job");
        }

        if (accountPost.Point < newPayment.UsedPoint)
        {
            throw new Exception("You not enough point to post job");
        }

        updateJobPost = await _jobPostRepository.UpdateJobPost(updateJobPost, updateJobPost.PayPerHourJob,
            updateJobPost.PayPerTaskJob, listPin, newPayment);

        var result = _mapper.Map<JobPostDetail>(updateJobPost);
        return result;
    }

    public async Task<JobPostDetail> DeleteJobPost(int jobPostId)
    {
        DataTier.Entities.JobPost deleteGuidepost = await _jobPostRepository.DeleteJobPost(jobPostId);
        var result = _mapper.Map<JobPostDetail>(deleteGuidepost);
        return result;
    }

    public async Task SwitchStatusJob(int jobPostId)
    {
        // _jobPostRepository
    }

    public async Task<JobPostDetail> EndJobPost(int jobPostId)
    {
        DataTier.Entities.JobPost deleteGuidepost = await _jobPostRepository.EndJobPost(jobPostId);
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
        // Sent noti
        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            { "type", "jobPost" },
            { "jobPostId", jobPostId.ToString() }
        };
        await PushNotification.SendMessage(jobPost.PublishedBy.ToString(), $"Bài đăng đã được duyệt",
            $"Bài đăng {jobPost.Title} đã được duyệt", data);

        return result;
    }

    public async Task<JobPostDetail> RejectJobPost(RejectJobPost rejectJobPost, int approvedBy)
    {
        var jobPost = await _jobPostRepository.RejectJobPost(rejectJobPost, approvedBy);
        var result = _mapper.Map<JobPostDetail>(jobPost);

        // Sent noti
        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            { "type", "jobPost" },
            { "jobPostId", jobPost.Id.ToString() }
        };
        await PushNotification.SendMessage(jobPost.PublishedBy.ToString(), $"Bài đăng của bạn đã bị từ chối",
            $"Bài đăng {jobPost.Title} không được duyệt", data);


        return result;
    }

    public async Task<JobPostDetail> StartJobPost(int jobPostId)
    {
        var jobPost = await _jobPostRepository.StartJob(jobPostId);
        var result = _mapper.Map<JobPostDetail>(jobPost);
        return result;
    }

    public async Task ExtendMaxFarmer(int jobPostId, int maxFarmer)
    {
        DataTier.Entities.JobPost jobPost = await _jobPostRepository.GetJobPostById(jobPostId);

        if (!jobPost.Type.Equals("PayPerHourJob"))
        {
            throw new MException(StatusCodes.Status400BadRequest, "Job is not Hour Job");
        }

        try
        {
            await _jobPostRepository.ExtendMaxFarmer(jobPostId, maxFarmer);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }

    public async Task<JobPostDetail> ExtendJobPostForLandowner(int jobPostId, DateTime dateTimeExtend, int usePoint = 0)
    {
        // var jobPost = await _jobPostRepository.GetJobPostById(jobPostId);
        // if (jobPost == null)
        // {
        //     throw new Exception("Job Post is not existed");
        // }
        //
        // if (jobPost.Status != (int?)JobPostEnum.JobPostStatus.Posted)
        // {
        //     throw new Exception("Job post is not posted");
        // }

        // DateTime dateExpired = jobPost.PublishedDate.Value.AddDays((double)jobPost.NumPublishDay - 1);

        // if (dateExpired >= dateTimeExtend)
        // {
        //     throw new Exception("Datetime extend not less than current end date");
        // }

        // int? numberOfDayBefore = jobPost.NumPublishDay;
        // int? dayExtend = dateTimeExtend.DayOfYear - dateExpired.DayOfYear;
        // jobPost.NumPublishDay = numberOfDayBefore + dayExtend;


        //--Get Setting From Json File
        // float pricePoint = float.Parse((_configRepository.GetConfigByKey("Point"))!);
        // float earnPoint = float.Parse((_configRepository.GetConfigByKey("EarnPoint"))!);

        //--Calculate Price
        // float aPriceJobPost = (float)(priceJobPost * (dayExtend));
        // float aPriceUsePoint = usePoint * pricePoint;

        //--Get more information to calculate
        // DataTier.Entities.Account accountPost = await _accountRepository.GetAccountByIdAsync((int)jobPost.PublishedBy);

        // PaymentHistory newPayment = new PaymentHistory
        // {
        // JobPostId = jobPost.Id,
        // CreatedBy = jobPost.PublishedBy,
        // Status = (int?)PaymentHistoryEnum.PaymentHistoryStatus.Paid,
        // CreatedDate = DateTime.Now,
        // ActualPrice = aPriceJobPost,
        // BalanceFluctuation = -(aPriceJobPost - aPriceUsePoint),
        // Balance = accountPost.Balance - (aPriceJobPost - aPriceUsePoint),
        // EarnedPoint = (int?)(aPriceJobPost / earnPoint),
        // UsedPoint = usePoint,
        // BelongedId = (int)jobPost.PublishedBy,
        // Message = $"Gia hạn bài đăng #{jobPost.Id} thêm {dayExtend} ngày",
        // PointPrice = (int?)pricePoint,
        // NumberPublishedDay = dayExtend 
        // };

        // if (newPayment.Balance < 0)
        // {
        //     throw new Exception("You not enough money to post job");
        // }
        //
        // if (accountPost.Point < newPayment.UsedPoint)
        // {
        //     throw new Exception("You not enough point to post job");
        // }

        // accountPost.Balance -= (aPriceJobPost - aPriceUsePoint);
        // accountPost.Point += (newPayment.EarnedPoint - usePoint);

        // await _jobPostRepository.ExtendJobPostForLandowner(jobPost, newPayment, accountPost);
        //     
        // var result = _mapper.Map<JobPostDetail>(jobPost);
        // return result;
        return null;
    }

    public async Task<IEnumerable<DateTime>> ListDateNoPin(DateTime publishedDate, int numberOfDayPublish,
        int postTypeId)
    {
        if (numberOfDayPublish <= 0) throw new Exception("Number of Publish is incorrect");

        ICollection<DateTime> result = new List<DateTime>();
        DateTime endPublishedDate = publishedDate.Date.AddDays(numberOfDayPublish);
        DateTime startDate = publishedDate;
        while (startDate.Date != endPublishedDate.Date)
        {
            result.Add(startDate);
            startDate = startDate.Date.AddDays(1);
        }

        IQueryable<PinDate> pinDates =
            _jobPostRepository.GetExistedPinDateOnRange(publishedDate, numberOfDayPublish, postTypeId);
        List<PinDate> list = await pinDates.ToListAsync();
        if (list.Count != 0)
        {
            var listPinDate = list.Select(pd => pd.PinDate1.Date);
            return result.Where(r => !listPinDate.Contains(r));
        }

        return result;
    }

    public async Task<int> MaxNumberOfPinDate(DateTime pinDate, int numberPublishDay, int postTypeId)
    {
        IQueryable<PinDate> pinDates = _jobPostRepository.GetNearPinDateByPinDate(pinDate, postTypeId);
        PinDate? nearPinDate = await pinDates.FirstOrDefaultAsync();
        if (nearPinDate == null) return numberPublishDay;
        return nearPinDate.PinDate1.DayOfYear - pinDate.DayOfYear;
    }

    public async Task<int> TotalPinDate(int jobPostId)
    {
        int result = await _jobPostRepository.TotalPinDate(jobPostId);
        return result;
    }

    public async Task PostingJob()
    {
        await _jobPostRepository.PostingJob();
    }

    public async Task ExpiringJob()
    {
        await _jobPostRepository.ExpiredJob();
    }

    public async Task OutOfDateJob()
    {
        await _jobPostRepository.ExpiredApproveJob();
    }

    public async Task StartingJob()
    {
        await _jobPostRepository.StartJob();
    }
}