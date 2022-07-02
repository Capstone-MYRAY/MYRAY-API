using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MYRAY.Business.DTOs.Message;
using MYRAY.Business.Services.Account;
using MYRAY.Business.Services.Hubs;
using MYRAY.Business.Services.Message;
using SignalRSwaggerGen.Attributes;

namespace MYRAY.Api.Hubs;

/// <summary>
/// Chat Hub
/// </summary>
[Authorize]
[SignalRHub]
public class ChatHub : Hub
{
    private readonly IMessageService _messageService;
    private readonly IConnectionService _connectionService;
    private readonly IAccountService _accountService;

    /// <summary>
    /// Initialize a new instance of Chat Hub
    /// </summary>
    /// <param name="messageService">Injection of <see cref="IMessageService"/></param>
    /// <param name="connectionService">Injection of <see cref="IConnectionService"/></param>
    /// <param name="accountService">Injection of <see cref="IAccountService"/></param>
    public ChatHub(
        IMessageService messageService, 
        IConnectionService connectionService, 
        IAccountService accountService)
    {
        _messageService = messageService;
        _connectionService = connectionService;
        _accountService = accountService;
    }

    /// <summary>
    /// Method to user invoke send message.
    /// </summary>
    /// <param name="newMessageRequest">Object contain info of new message</param>
    public async Task SendMessage(NewMessageRequest newMessageRequest)
    {
        ObjectInfo? info = _connectionService.GetConnectionById(newMessageRequest.ToId);
        
        if(info != null)
        {
            Console.WriteLine($"Send message to {info.Account.Fullname} - {newMessageRequest.Content}");
            await Clients.Client(info.ConnectionId).SendAsync("chat", info, newMessageRequest);

        }
        else
        {
            Console.WriteLine(" User is offline");
        }
        //- Store message to DB
        await _messageService.StoreNewMessage(newMessageRequest);
        Console.WriteLine("Message send");
    }
    
    
    /// <summary>
    /// On user connected to hub
    /// </summary>
    /// <exception cref="Exception"></exception>
    public override async Task OnConnectedAsync()
    {
        try
        {
            if (Context.User!.Identity != null)
            {
                // string? phoneNumber = Context.User.Identity.Name;
                int id = int.Parse(Context.User.FindFirst("id")!.Value);
                await _connectionService.AddConnection(id, Context.ConnectionId, _accountService);
                Console.WriteLine($"Connected: {Context.ConnectionId}");
                await base.OnConnectedAsync();
            }
            else
            {
                throw new Exception("Token invalid");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await base.OnDisconnectedAsync(e);
        }
    }

    /// <summary>
    /// On User disconnected to hub
    /// </summary>
    /// <param name="exception">Exception obj</param>
    /// <returns></returns>
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _connectionService.DeleteConnection(Context.ConnectionId);
        Console.WriteLine($"Disconnected: {Context.ConnectionId}");
        return base.OnDisconnectedAsync(exception);
    }
    
    
}