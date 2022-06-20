using MYRAY.Business.Enums;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.JobPost;

public interface IJobPostRepository
{
    IQueryable<DataTier.Entities.JobPost> GetJobPosts(int? publishBy = null);
    IQueryable<DataTier.Entities.JobPost> GetPinPost();
    Task<DataTier.Entities.JobPost> GetJobPostById(int id);

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

    void DeletePinDate(ICollection<PinDate> pinDates);

    void DeletePaymentHistory(int jobPostId, int publishId);
    IQueryable GetPinDateByJobPost(int id);
    
    Task<DataTier.Entities.JobPost> ApproveJobPost(int jobPostId, int approvedBy);
    
    Task<DataTier.Entities.JobPost> RejectJobPost(int jobPostId, int approvedBy);


}