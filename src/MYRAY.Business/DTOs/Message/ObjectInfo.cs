using MYRAY.Business.DTOs.Account;

namespace MYRAY.Business.DTOs.Message;

public class ObjectInfo
{
    public ObjectInfo(string connectionId, GetAccountDetail account)
    {
        ConnectionId = connectionId;
        Account = account;
    }

    public string ConnectionId { get; set; }
    public GetAccountDetail Account { get; set; }
}