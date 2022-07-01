using MYRAY.Business.DTOs.Account;
using MYRAY.Business.DTOs.Message;
using MYRAY.Business.Services.Account;

namespace MYRAY.Business.Services.Hubs;

public class ConnectionService : IConnectionService
{
    private readonly Dictionary<int, ObjectInfo> _onlineUser;

    public ConnectionService()
    {
       
        _onlineUser = new Dictionary<int, ObjectInfo>();
    }

    public ObjectInfo? GetConnectionById(int toId)
    {
        if (_onlineUser.ContainsKey(toId))
        {
            return _onlineUser[toId];
        }

        return null;
    }

    public int GetLength()
    {
        return _onlineUser.Count;
    }

    public async Task AddConnection(int fromId, string connectionId, IAccountService accountService)
    {
        try
        {
            if (!_onlineUser.ContainsKey(fromId))
            {
                GetAccountDetail account = await accountService.GetAccountByIdAsync(fromId);
                ObjectInfo info = new ObjectInfo(connectionId, account);
                _onlineUser.Add(fromId, info);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Account is not existed");
        }
    }

    public async Task DeleteConnection(string connectionId)
    {
        var key = _onlineUser.FirstOrDefault(item => item.Value.ConnectionId == connectionId).Key;
           _onlineUser.Remove(key);
    }
}