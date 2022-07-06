using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.JobPost;
using MYRAY.Business.Enums;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Services.JobPost;

public interface IJobPostService
{
    ResponseDto.CollectiveResponse<JobPostDetail> GetJobPosts(
        SearchJobPost searchJobPost,
        PagingDto pagingDto,
        SortingDto<JobPostEnum.JobPostSortCriteria> sortingDto,
        int? publishId = null,
        bool isFarmer = false);

    Task<JobPostDetail> GetJobPostById(int id);

    Task<JobPostDetail> CreateJobPost(CreateJobPost jobPost, int publishedBy);

    Task<JobPostDetail> UpdateJobPost(UpdateJobPost jobPost, int publishedBy);

    Task<JobPostDetail> DeleteJobPost(int jobPostId);
    Task<JobPostDetail> CancelJobPost(int jobPostId);

    Task<JobPostDetail> ApproveJobPost(int jobPostId, int approvedBy);
    Task<JobPostDetail> RejectJobPost(RejectJobPost rejectJobPost, int approvedBy);

    Task<JobPostDetail> StartJobPost(int jobPostId);

    Task<JobPostDetail> ExtendJobPostForLandowner(int jobPostId, DateTime dateTimeExtend, int usePoint = 0);

    Task<IEnumerable<DateTime>> ListDateNoPin(DateTime publishedDate, int numberOfDayPublish, int postTypeId);

    Task<int> MaxNumberOfPinDate(DateTime pinDate,int numberPublishDay, int postTypeId);

    Task PostingJob();
    Task ExpiringJob();
    Task StartingJob();

}