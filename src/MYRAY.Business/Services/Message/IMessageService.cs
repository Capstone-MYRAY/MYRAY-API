using MYRAY.Business.DTOs.Message;

namespace MYRAY.Business.Services.Message;

public interface IMessageService
{
    Task StoreNewMessage(NewMessageRequest newMessageRequest);
    Task<List<MessageDetail>> GetMessageByConventionId(string conventionId);
    
    Task<List<MessageJobPost>?> GetListMessageForLandowner(int landownerId);
    Task<List<MessageFarmer>?> GetListMessageForFarmer(int farmerId);

    Task MarkRead(int accountId, string conventionId);
}