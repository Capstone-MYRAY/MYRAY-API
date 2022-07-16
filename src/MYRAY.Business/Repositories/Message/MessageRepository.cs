using Microsoft.EntityFrameworkCore;
using MYRAY.Business.DTOs.Message;
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
        _appliedJobRepository =
            _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.AppliedJob>()!;
    }

    public async Task<DataTier.Entities.Message> CreateNewMessage(DataTier.Entities.Message message)
    {
        DataTier.Entities.JobPost jobPost = (await _jobPostRepository.GetByIdAsync(message.JobPostId!))!;

        if (jobPost == null)
        {
            throw new Exception("Job post is not existed");
        }


        await _messageRepository.InsertAsync(message);
        await _contextFactory.SaveAllAsync();

        return message;
    }

    public IQueryable<DataTier.Entities.Message> GetMessageByConventionId(string conventionId)
    {
        IQueryable<DataTier.Entities.Message> query = _messageRepository.Get(m => m.ConventionId == conventionId)
            .OrderByDescending(m => m.CreatedDate);
        return query;
    }

    public async Task<List<MessageJobPost>> GetMessageByLandowner(int landownerId)
    {
        List<MessageJobPost>? result = null;
        IQueryable<int?> queryJobPostId = _messageRepository
            .Get()
            .AsNoTracking()
            .Where(m => m.JobPost.PublishedBy == landownerId)
            .Select(m => m.JobPostId).Distinct();
        List<int?> listJobPostId = await queryJobPostId.ToListAsync();
        if (!listJobPostId.Any())
            result = new List<MessageJobPost>();

        IQueryable<DataTier.Entities.JobPost> queryJobPost = _jobPostRepository.Get()
            .AsNoTracking()
            .Where(j => listJobPostId.Contains(j.Id));
        List<DataTier.Entities.JobPost> listJobPost = await queryJobPost.ToListAsync();
        
        foreach (var jobPost in listJobPost)
        {
            string conventionId = jobPost.Id + landownerId.ToString();
            IQueryable<DataTier.Entities.Message> queryFarmer = _messageRepository
                .Get()
                .AsNoTracking()
                .OrderByDescending(m => m.CreatedDate)
                .Where(m => m.ConventionId.Contains(conventionId)
                            && m.FromId != landownerId)
                .DistinctBy(m => m.ConventionId);

        //    MessageJobPost oneJobPost = new MessageJobPost()
        //     {
        //         JobPostId = jobPost.Id,
        //         JobPostTitle = jobPost.Title,
        //         Farmers = await queryFarmer.ToListAsync();
        //     }
        //     result!.Add();
        }

        return null;
    }
}