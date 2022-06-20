using MYRAY.Business.Repositories.Interface;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.Feedback;

public class FeedbackRepository : IFeedbackRepository
{
    private readonly IDbContextFactory _contextFactory;
    private readonly IBaseRepository<DataTier.Entities.Feedback> _feedbackRepository;

    public FeedbackRepository(IDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
        _feedbackRepository = _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.Feedback>()!;
    }

    public IQueryable<DataTier.Entities.Feedback> GetFeedbacks()
    {
        IQueryable<DataTier.Entities.Feedback> query = _feedbackRepository.Get();
        return query;
    }

    public async Task<DataTier.Entities.Feedback> GetFeedbackById(int id)
    {
        DataTier.Entities.Feedback feedback =
            (await _feedbackRepository.GetFirstOrDefaultAsync(r => r.Id == id))!;
        return feedback;
    }

    public async Task<DataTier.Entities.Feedback> CreateFeedback(DataTier.Entities.Feedback feedback)
    {
        await _feedbackRepository.InsertAsync(feedback);

        await _contextFactory.SaveAllAsync();

        return feedback;
    }

    public async Task<DataTier.Entities.Feedback> UpdateFeedback(DataTier.Entities.Feedback feedback)
    {
        
        _feedbackRepository.Modify(feedback);

        await _contextFactory.SaveAllAsync();
        return feedback;
    }

    public async Task<DataTier.Entities.Feedback> DeleteFeedback(int id)
    {
        DataTier.Entities.Feedback feedback = _feedbackRepository.GetById(id)!;
        if (feedback == null)
        {
            throw new Exception("Feedback is not existed");
        }
        
        _feedbackRepository.Delete(feedback);

        await _contextFactory.SaveAllAsync();

        return feedback;
    }
}