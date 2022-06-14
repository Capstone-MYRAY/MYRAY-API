using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYRAY.Api.Constants;
using MYRAY.Business.Constants;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Account;
using MYRAY.Business.Enums;
using MYRAY.Business.Exceptions;
using MYRAY.Business.Helpers;
using MYRAY.Business.Services.Account;

namespace MYRAY.Api.Controllers;
/// <summary>
/// Handle request related to Account
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes(MediaType.ApplicationJson)]
[Produces(MediaType.ApplicationJson)]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountController"/> class.
    /// </summary>
    /// <param name="accountService">Injection of <see cref="IAccountService"/></param>
    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    /// <summary>
    /// [Authenticated user] Endpoint for get all account with condition
    /// </summary>
    /// <param name="searchAccountDto">An object contains filter criteria.</param>
    /// <param name="sortingDto">An object contains sorting criteria.</param>
    /// <param name="pagingDto">An object contains paging criteria.</param>
    /// <returns>List of account</returns>
    /// <response code="200">Returns the list of account.</response>
    /// <response code="204">Returns if list of account is empty.</response>
    /// <response code="401">Returns if token is access denied.</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDto.CollectiveResponse<GetAccountDetail>),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> GetAccounts(
        [FromQuery]SearchAccountDto searchAccountDto,
        [FromQuery]SortingDto<AccountEnum.AccountSortCriteria> sortingDto,
        [FromQuery]PagingDto pagingDto)
    {
        ResponseDto.CollectiveResponse<GetAccountDetail> result =
            _accountService.GetAccounts(pagingDto, sortingDto, searchAccountDto);
        if (result == null)
        {
            return Task.FromResult<IActionResult>(NoContent());
        }
        return Task.FromResult<IActionResult>(Ok(result));
    }
    
    /// <summary>
    /// [All] Endpoint for get account information by Identifier.
    /// </summary>
    /// <param name="accountId">An id of account</param>
    /// <returns>An account</returns>
    /// <response code="200">Returns the account.</response>
    /// <response code="204">Returns if account is not existed.</response>
    /// <response code="401">Returns if token is access denied</response>
    [HttpGet("{accountId}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GetAccountDetail),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorCustomMessage),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAccountByIdAsync(int? accountId)
    {
        if (accountId == null || !int.TryParse(accountId.ToString(), out _))
        {
            return BadRequest(new ErrorCustomMessage
            {
                Message = "Account ID is null or not number",
                Target = nameof(accountId)
            });
        }
            
        GetAccountDetail result = await _accountService.GetAccountByIdAsync((int)accountId);
        
        if (result == null)
        {
            return NoContent();
        }
        return Ok(result);
    }
    
    /// <summary>
    /// [All] Endpoint for get account information by phone number.
    /// </summary>
    /// <param name="phoneNumber">An phone number of account</param>
    /// <returns>An account</returns>
    /// <response code="200">Returns the account.</response>
    /// <response code="204">Returns if account is not existed.</response>
    /// <response code="401">Returns if token is access denied</response>
    [HttpGet("phone/{phoneNumber}")]
    
    [AllowAnonymous]
    [ProducesResponseType(typeof(GetAccountDetail),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorCustomMessage),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAccountByPhoneNumberAsync(string? phoneNumber)
    {
        if (phoneNumber == null || !phoneNumber.IsValidPhoneNumber())
        {
            return BadRequest(new ErrorCustomMessage
            {
                Message = "Phone Number is null or invalid",
                Target = nameof(phoneNumber)
            });
        }
            
        GetAccountDetail result = await _accountService.GetAccountByPhoneNumberAsync(phoneNumber.ConvertVNPhoneNumber());
        
        if (result == null)
        {
            return NoContent();
        }
        return Ok(result);
    }

    /// <summary>
    /// [Moderator] Endpoint for create account
    /// </summary>
    /// <param name="accountDto">Object contains account dto</param>
    /// <returns>IActionResult</returns>
    /// <exception cref="Exception">Account is empty data</exception>
    /// <exception cref="Exception">Error if input is empty</exception>
    /// <response code="201">Returns the created account</response>
    /// <response code="400">Returns if account input is empty or create error</response>
    /// <response code="401">Returns if token is access denied</response>
    [HttpPost]
    [Authorize(Roles = UserRole.MODERATOR)]
    [ProducesResponseType(typeof(GetAccountDetail), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAccountAsync(
        [FromBody] InsertAccountDto accountDto)
    {
        try
        {
            if (accountDto == null)
            {
                throw new Exception("Account is empty data");
            }
            GetAccountDetail result = await _accountService.CreateAccountAsync(accountDto);
    
            return Created(string.Empty,result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new ErrorCustomMessage {Message = e.Message, Target = nameof(accountDto)});
        }
    }

    
    /// <summary>
    /// [Authenticated user] Endpoint for update account
    /// </summary>
    /// <param name="updateAccountDto">An object contains update information</param>
    /// <returns>An account updated</returns>
    /// <response code="200">Returns the account updated</response>
    /// <response code="400">Returns if input account information empty</response>
    /// <response code="404">Returns if area account is not existed.</response>
    /// <response code="401">Returns if token is access denied</response>
    [HttpPut]
    [Authorize]
    [ProducesResponseType(typeof(UpdateAccountDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAccountAsync(UpdateAccountDto updateAccountDto)
    {
        try
        {
            UpdateAccountDto updateAccount = await _accountService.UpdateAccountAsync(updateAccountDto);

            return Ok(updateAccount);
        }
        catch (MException e)
        {
            return BadRequest(e);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound(new MException(StatusCodes.Status404NotFound, e.Message, nameof(updateAccountDto.Id)));
        }
    }

    /// <summary>
    /// [Moderator] Endpoint for delete account.
    /// </summary>
    /// <param name="accountId">Id of  account</param>
    /// <returns>Async function</returns>
    /// <response code="204">Returns the account deleted</response>
    /// <response code="404">Returns if account is not existed.</response>
    [Authorize(Roles = UserRole.MODERATOR)]
    [HttpDelete("{accountId}")]
    public async Task<IActionResult> DeleteAccountAsync(int? accountId)
    {
        try
        {
            await _accountService.DeleteAccountByIdAsync(accountId);

            return NoContent();
        }
        catch (Exception e)
        {
            return NotFound(e);
        }
    }

    /// <summary>
    /// [Moderator] Endpoint for top up account.
    /// </summary>
    /// <param name="accountId">Id of  account</param>
    /// <param name="topUp">Money to top up</param>
    /// <returns>Async function</returns>
    /// <response code="200">Returns the account top up success</response>
    /// <response code="400">Returns if account is not existed or invalid information.</response>
    [HttpPost]
    [Authorize(Roles = UserRole.MODERATOR)]
    [Route("topup")]
    public async Task<IActionResult> TopUpAccount(int? accountId, float topUp)
    {
        try
        {
            if (accountId == null)
                return BadRequest("Invalid account ID");

           GetAccountDetail result = await _accountService.TopUpAccountByIdAsync(accountId, topUp);
           return Ok(result);
           
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Moderator] Endpoint for ban account.
    /// </summary>
    /// <param name="accountId">Id of  account</param>
    /// <returns>Async function</returns>
    /// <response code="204">Returns the account top up success</response>
    /// <response code="400">Returns if account is not existed or invalid id.</response>
    [HttpDelete("ban/{accountId}")]
    [Authorize(Roles = UserRole.MODERATOR)]
    public async Task<IActionResult> BanAccount(int? accountId)
    {
        try
        {
            if (accountId == null) return BadRequest("Id is not empty");

            await _accountService.BanAccountByIdAsync(accountId);
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
}