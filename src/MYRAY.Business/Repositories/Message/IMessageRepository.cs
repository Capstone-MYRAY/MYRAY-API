namespace MYRAY.Business.Repositories.Message;

public interface IMessageRepository
{
    Task<DataTier.Entities.Message> CreateNewMessage(DataTier.Entities.Message message);
}