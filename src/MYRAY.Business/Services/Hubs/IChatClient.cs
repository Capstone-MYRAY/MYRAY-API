using MYRAY.Business.DTOs.Message;

namespace MYRAY.Business.Services.Hubs;

public interface IChatClient
{
    Task<NewMessageRequest> OnNewMessage(DataTier.Entities.Message message);
}