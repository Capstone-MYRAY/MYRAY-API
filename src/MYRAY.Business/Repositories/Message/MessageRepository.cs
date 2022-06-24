using MYRAY.Business.Enums;
using MYRAY.Business.Repositories.Interface;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.Message;

public class MessageRepository : IMessageRepository
{
    private readonly IDbContextFactory _contextFactory;
    private readonly IBaseRepository<DataTier.Entities.Message> _messageRepository;
    private readonly IBaseRepository<DataTier.Entities.JobPost> _jobPostRepository;
    private readonly IBaseRepository<DataTier.Entities.AppliedJob> _appliedJobRepository;

    public MessageRepository(IDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
        _messageRepository = _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.Message>()!;
        _jobPostRepository = _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.JobPost>()!;
        _appliedJobRepository = _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.AppliedJob>()!;
    }

    public async Task<DataTier.Entities.Message> CreateNewMessage(DataTier.Entities.Message message)
    {
        DataTier.Entities.JobPost jobPost = (await _jobPostRepository.GetByIdAsync(message.JobPostId!))!;

        if (jobPost == null)
        {
            throw new Exception("Job post is not existed");
        }

        DataTier.Entities.AppliedJob appliedJob =
            (await _appliedJobRepository.GetFirstOrDefaultAsync(ap => ap.JobPostId == jobPost.Id && ap.AppliedBy == message.FromId))!;

        if (appliedJob == null || appliedJob.Status != (int?)AppliedJobEnum.AppliedJobStatus.Approve)
        {
            throw new Exception("You not in job post");
        }

        await _messageRepository.InsertAsync(message);
        await _contextFactory.SaveAllAsync();

        return message;
    }
}