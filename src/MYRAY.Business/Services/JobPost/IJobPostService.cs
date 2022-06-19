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
        int? publishId = null);

    Task<JobPostDetail> GetJobPostById(int id);

    Task<JobPostDetail> CreateJobPost(CreateJobPost jobPost, int publishedBy);

    Task<JobPostDetail> UpdateJobPost(UpdateJobPost jobPost, int publishedBy);

    Task<JobPostDetail> DeleteJobPost(int jobPostId);

    Task<JobPostDetail> ApproveJobPost(int jobPostId);
    Task<JobPostDetail> RejectJobPost(int jobPostId);

}