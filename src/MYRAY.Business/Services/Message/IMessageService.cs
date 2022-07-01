using MYRAY.Business.DTOs.Message;

namespace MYRAY.Business.Services.Message;

public interface IMessageService
{
    Task StoreNewMessage(NewMessageRequest newMessageRequest);
    Task<List<DataTier.Entities.Message>> GetMessageByConventionId(string conventionId);
}