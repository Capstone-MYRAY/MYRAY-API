namespace MYRAY.Business.Repositories.Feedback;

public interface IFeedbackRepository
{
    IQueryable<DataTier.Entities.Feedback> GetFeedbacks();
    Task<DataTier.Entities.Feedback> GetFeedbackById(int id);
    Task<DataTier.Entities.Feedback?> GetOneFeedback(int jobPostId, int belongId, int createId);
    Task<DataTier.Entities.Feedback> CreateFeedback(DataTier.Entities.Feedback feedback);
    Task<DataTier.Entities.Feedback> UpdateFeedback(DataTier.Entities.Feedback feedback);
    Task<DataTier.Entities.Feedback> DeleteFeedback(int id);
}