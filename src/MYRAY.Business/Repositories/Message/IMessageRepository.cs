using MYRAY.Business.DTOs.Message;

namespace MYRAY.Business.Repositories.Message;

public interface IMessageRepository
{
    Task<DataTier.Entities.Message> CreateNewMessage(DataTier.Entities.Message message);
    IQueryable<DataTier.Entities.Message> GetMessageByConventionId(string conventionId);

    Task<List<MessageJobPost>?> GetMessageByLandowner(int landownerId);
    Task<List<MessageFarmer>> GetMessageByFarmer(int farmerId);

    Task MarkRead(int accountId, string conventionId);
}