using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MYRAY.Business.DTOs.Message;
using MYRAY.Business.Repositories.Interface;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.Message;

public class MessageRepository : IMessageRepository
{
    private readonly IDbContextFactory _contextFactory;
    private readonly IBaseRepository<DataTier.Entities.Message> _messageRepository;
    private readonly IBaseRepository<DataTier.Entities.JobPost> _jobPostRepository;
    private readonly IBaseRepository<DataTier.Entities.AppliedJob> _appliedJobRepository;
    private readonly IMapper _mapper;

    public MessageRepository(IDbContextFactory contextFactory, IMapper mapper)
    {
        _contextFactory = contextFactory;
        _messageRepository = _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.Message>()!;
        _jobPostRepository = _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.JobPost>()!;
        _appliedJobRepository =
            _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.AppliedJob>()!;
        _mapper = mapper;
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

    public async Task<List<MessageJobPost>?> GetMessageByLandowner(int landownerId)
    {
        List<MessageJobPost>? result = null;
        IQueryable<DataTier.Entities.JobPost> queryJobPost = _messageRepository
            .Get()
            .AsNoTracking()
            .Where(m => m.JobPost.PublishedBy == landownerId)
            .Select(m => m.JobPost).Distinct();
        List<DataTier.Entities.JobPost> listJobPost = await queryJobPost.ToListAsync();
        if (queryJobPost.Any())
            result = new List<MessageJobPost>();

        if(listJobPost.Any())
            foreach (var jobPost in listJobPost)
            {
                string conventionId = jobPost.Id + landownerId.ToString();
                Expression<Func<DataTier.Entities.Message, object>> expFrom = message => message.From; 
                Expression<Func<DataTier.Entities.Message, object>> expTo = message => message.To; 
                IEnumerable<DataTier.Entities.Message> listFarmer = _messageRepository
                    .Get(includeProperties: new []{expFrom, expTo})
                    .AsNoTracking()
                    .OrderByDescending(m => m.CreatedDate)
                    .Where(m => m.ConventionId.Contains(conventionId))
                    .AsEnumerable()
                    .DistinctBy(m => m.ConventionId);
                List<DataTier.Entities.Message> messageToFarmer = listFarmer.ToList();
                List<Farmer> bindListFarmer = new List<Farmer>();
                foreach (var lastMessage in messageToFarmer)
                {
                    DataTier.Entities.Account accountFarmer =
                        lastMessage.FromId != landownerId ? lastMessage.From : lastMessage.To;
                    Farmer oneFarmer = new Farmer()
                    {
                        Id = accountFarmer.Id,
                        Image = accountFarmer.ImageUrl,
                        Name = accountFarmer.Fullname,
                        ConventionId = jobPost.Id + landownerId.ToString() + accountFarmer.Id,
                        LastMessage = _mapper.Map<MessageDetail>(lastMessage)
                    };
                    if (lastMessage.FromId == landownerId)
                    {
                        oneFarmer.LastMessage.IsRead = true;
                    }
                    bindListFarmer.Add(oneFarmer);
                }

                MessageJobPost oneJobPost = new MessageJobPost()
                {
                    JobPostId = jobPost.Id,
                    JobPostTitle = jobPost.Title,
                    Farmers = bindListFarmer.OrderByDescending(b => b.LastMessage.CreatedDate).ToList(),
                    LastMessageTime = bindListFarmer
                        .OrderByDescending(bld => bld.LastMessage.CreatedDate)
                        .Select(blf =>blf.LastMessage.CreatedDate).FirstOrDefault()
                };
                result!.Add(oneJobPost);
            }

        result = result.OrderByDescending(r => r.LastMessageTime).ToList();
        return result;
    }

    public async Task<List<MessageFarmer>> GetMessageByFarmer(int farmerId)
    {
        string farmerIdS = farmerId.ToString();
        Expression<Func<DataTier.Entities.Message, object>> expPublish = post => post.JobPost.PublishedByNavigation;
        IEnumerable<DataTier.Entities.JobPost> listConventions = _messageRepository
            .Get(includeProperties: new []{expPublish})
            .AsNoTracking()
            .Where(m => m.ConventionId.EndsWith(farmerIdS))
            .AsEnumerable()
            .DistinctBy(m => m.ConventionId)
            .Select(m => m.JobPost);

        List<MessageFarmer> result = new List<MessageFarmer>();
        foreach (var jobPost in listConventions)
        {
            string conventionId = jobPost.Id + jobPost.PublishedBy.ToString() + farmerId;
            IQueryable<DataTier.Entities.Message> lastMessage = _messageRepository
                .Get(m => m.ConventionId.Equals(conventionId))
                .OrderByDescending(m => m.CreatedDate);
            DataTier.Entities.Message last = await lastMessage.FirstOrDefaultAsync();
            result.Add(new MessageFarmer()
            {
                Id = jobPost.Id,
                Title = jobPost.Title,
                PublishedBy = jobPost.PublishedByNavigation.Fullname,
                PublishedId = jobPost.PublishedBy,
                AvatarUrl = jobPost.PublishedByNavigation.ImageUrl,
                LastMessageTime = last.CreatedDate,
                IsRead = last.FromId == farmerId ? true : last.IsRead
            });
        }

        result = result.OrderByDescending(r => r.LastMessageTime).ToList();
        return result;
    }

    public async Task MarkRead(int accountId, string conventionId)
    {
        IQueryable<DataTier.Entities.Message> message = _messageRepository.Get(m => m.ConventionId.Equals(conventionId)
            && m.ToId == accountId);
        List<DataTier.Entities.Message> list = await message.ToListAsync();
        foreach (var markMessage in list)
        {
            markMessage.IsRead = true;
        }
        await _contextFactory.SaveAllAsync();
    }
}