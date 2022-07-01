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

    public async Task<List<DataTier.Entities.Message>> GetMessageByConventionId(string conventionId)
    {
        List<DataTier.Entities.Message> result = await _messageRepository.GetMessageByConventionId(conventionId).ToListAsync();
        return result;
    }
}