using MYRAY.Business.DTOs.Message;
using MYRAY.Business.Services.Account;

namespace MYRAY.Business.Services.Hubs;

public interface IConnectionService
{
    ObjectInfo? GetConnectionById(int toId);
    int GetLength();
    Task AddConnection(int fromId, string connectionId,  IAccountService accountService);
    Task DeleteConnection(string connectionId);
}