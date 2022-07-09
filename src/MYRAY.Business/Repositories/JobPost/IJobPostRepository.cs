using MYRAY.Business.DTOs.JobPost;
using MYRAY.Business.DTOs.TreeJob;
using MYRAY.Business.Enums;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.JobPost;

public interface IJobPostRepository
{
    IQueryable<DataTier.Entities.JobPost> GetJobPosts(int? publishBy = null);
    IQueryable<DataTier.Entities.JobPost> GetPinPost();

    IQueryable<PinDate> GetExistedPinDateOnRange(DateTime publishedDate, int numberPublishDay, int postTypeId);
    IQueryable<PinDate> GetNearPinDateByPinDate(DateTime pinDate, int postTypeId);
    IQueryable<DataTier.Entities.JobPost> GetJobAvailableByGardenId(int gardenId);
    IQueryable<MYRAY.DataTier.Entities.JobPost> GetInProgressJobPost();
    Task<DataTier.Entities.JobPost> GetJobPostById(int id);

    Task<PayPerHourJob> GetPayPerHourJob(int jobPostId);

    Task<DataTier.Entities.JobPost> CreateJobPost(
        DataTier.Entities.JobPost jobPost, 
        PayPerHourJob? payPerHourJob,
        PayPerTaskJob? payPerTaskJob,
        ICollection<PinDate>? pinDates,
        DataTier.Entities.PaymentHistory newPayment);
    Task<DataTier.Entities.JobPost> UpdateJobPost(DataTier.Entities.JobPost jobPost, 
        PayPerHourJob? payPerHourJob, 
        PayPerTaskJob? payPerTaskJob, 
        ICollection<PinDate>? pinDates,
        DataTier.Entities.PaymentHistory newPayment);
    Task<DataTier.Entities.JobPost> DeleteJobPost(int id);

    Task<DataTier.Entities.JobPost> CancelJobPost(int id);

    void DeletePinDate(ICollection<PinDate> pinDates);

    void ChangePaymentHistory(int jobPostId, int publishId);
    IQueryable GetPinDateByJobPost(int id);
    
    Task<DataTier.Entities.JobPost> ApproveJobPost(int jobPostId, int approvedBy);
    
    Task<DataTier.Entities.JobPost> RejectJobPost(RejectJobPost rejectJobPost, int approvedBy);

    Task<DataTier.Entities.JobPost> StartJob(int jobPostId);

    Task<DataTier.Entities.JobPost> ExtendJobPostForLandowner(
        DataTier.Entities.JobPost jobPost, 
        DataTier.Entities.PaymentHistory newPayment,
        DataTier.Entities.Account account);

    Task<int> TotalPinDate(int jobPostId);

    Task PostingJob();

    Task ExpiredJob();

    Task StartJob();

    Task SaveChange();
}