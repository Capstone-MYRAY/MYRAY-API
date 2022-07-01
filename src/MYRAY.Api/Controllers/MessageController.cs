using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYRAY.Api.Constants;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Account;
using MYRAY.Business.Enums;
using MYRAY.Business.Services.Message;
using MYRAY.DataTier.Entities;

namespace MYRAY.Api.Controllers;
/// <summary>
/// Handle request related to Message
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes(MediaType.ApplicationJson)]
[Produces(MediaType.ApplicationJson)]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageController"/> class.
    /// </summary>
    /// <param name="messageService"></param>
    public MessageController(IMessageService messageService)
    {
        _messageService = messageService;
    }
    
    /// <summary>
    /// [Authenticated user] Endpoint for get all account with condition
    /// </summary>
    /// <param name="conventionId">String conventionId</param>
    /// <returns>List of account</returns>
    /// <response code="200">Returns the list of account.</response>
    /// <response code="204">Returns if list of account is empty.</response>
    /// <response code="401">Returns if token is access denied.</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(List<Message>),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAccounts(
        [FromQuery]string conventionId)
    {
        List<Message> result = await _messageService.GetMessageByConventionId(conventionId);
        if (result == null)
        {
            return NoContent();
        }
        return Ok(result);
    }
}