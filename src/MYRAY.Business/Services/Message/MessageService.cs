using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MYRAY.Business.DTOs.Account;
using MYRAY.Business.DTOs.Message;
using MYRAY.Business.Repositories.Message;
using MYRAY.Business.Services.Account;

namespace MYRAY.Business.Services.Message;

public class MessageService : IMessageService
{
    private readonly IMapper _mapper;
    private readonly IMessageRepository _messageRepository;
    private readonly IAccountService _accountService;

    public MessageService(IMapper mapper, IMessageRepository messageRepository, IAccountService accountService)
    {
        _mapper = mapper;
        _messageRepository = messageRepository;
        _accountService = accountService;
    }

    public async Task StoreNewMessage(NewMessageRequest newMessageRequest)
    {
        DataTier.Entities.Message newMessage = _mapper.Map<DataTier.Entities.Message>(newMessageRequest);
        newMessage.CreatedDate = DateTime.Now;
        GetAccountDetail fromAccount = await _accountService.GetAccountByIdAsync(newMessageRequest.FromId);
        string conventionId = newMessageRequest.JobPostId.ToString();
        if (fromAccount.RoleId == 3)
        {
            conventionId += fromAccount.Id.ToString() + newMessageRequest.ToId;
        }
        else
        {
            conventionId += newMessageRequest.ToId.ToString() + fromAccount.Id;
        }

        newMessage.ConventionId = conventionId;

        await _messageRepository.CreateNewMessage(newMessage);
        
    }

    public async Task<List<MessageDetail>> GetMessageByConventionId(string conventionId)
    {
        IQueryable<DataTier.Entities.Message> query = _messageRepository.GetMessageByConventionId(conventionId);
        IQueryable<MessageDetail> result = _mapper.ProjectTo<MessageDetail>(query);
        return await result.ToListAsync();
    }

    public async Task<List<MessageJobPost>?> GetListMessageForLandowner(int landownerId)
    {
        return await _messageRepository.GetMessageByLandowner(landownerId);
    }

    public async Task<List<MessageFarmer>?> GetListMessageForFarmer(int farmerId)
    {
        return await _messageRepository.GetMessageByFarmer(farmerId);
    }

    public async Task MarkRead(int accountId, string conventionId)
    {
        await _messageRepository.MarkRead(accountId, conventionId);
    }
}