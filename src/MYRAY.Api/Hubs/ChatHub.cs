using Microsoft.AspNetCore.SignalR;
using MYRAY.Business.Services.Hubs;
using MYRAY.Business.Services.Message;

namespace MYRAY.Api.Hubs;

public class ChatHub : Hub
{
    private readonly IMessageService _messageService;

    public ChatHub(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}