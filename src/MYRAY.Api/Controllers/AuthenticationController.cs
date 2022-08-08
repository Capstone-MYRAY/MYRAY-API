using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYRAY.Api.Constants;
using MYRAY.Business.DTOs.Account;
using MYRAY.Business.DTOs.Authentication;
using MYRAY.Business.Exceptions;
using MYRAY.Business.Helpers;
using MYRAY.Business.Services.Account;
using MYRAY.Business.Services.Authentication;
using Telnyx;

namespace MYRAY.Api.Controllers;
/// <summary>
/// Handle request related to Authentication
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes(MediaType.ApplicationJson)]
[Produces(MediaType.ApplicationJson)]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authentication;
    private readonly IAccountService _accountService;

    /// <summary>
    /// Initialize a new instance <see cref="AuthenticationController"/> class.
    /// </summary>
    /// <param name="authentication">Injection of <see cref="IAuthenticationService"/></param>
    /// <param name="accountService">Injection of <see cref="IAccountService"/> </param>
    public AuthenticationController(IAuthenticationService authentication, IAccountService accountService)
    {
        _authentication = authentication;
        _accountService = accountService;
    }

    /// <summary>
    /// [Unauthenticated user] Endpoint for login 
    /// </summary>
    /// <param name="bodyDto">An object contains login information</param>
    /// <returns>Token and refresh token</returns>
    /// <response code="200">Returns access token and refresh token.</response>
    /// <response code="400">Returns if login information is empty.</response>
    /// <response code="400">Returns if invalid login information.</response>
    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthenticatedResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string),StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto? bodyDto)
    {
        if (bodyDto is null)
        {
            return BadRequest("Invalid Request");
        }

        AuthenticatedResponse? response ;
        try
        {
            response = await _authentication.LoginByPhoneAsync(bodyDto.PhoneNumber!.ConvertVNPhoneNumber(),
                bodyDto.Password!);
        }
        catch (MException e)
        {
            return BadRequest(e.Message);
        }

        return Ok(response);
    }

    /// <summary>
    /// [Unauthenticated user] Endpoint for refresh token 
    /// </summary>
    /// <param name="requestBody">An object contains refresh information</param>
    /// <returns>New Token and new refresh token</returns>
    /// <response code="200">Returns new access token and new refresh token.</response>
    /// <response code="400">Returns if refresh token invalid or expired.</response>
    [HttpPost]
    [Route("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthenticatedResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Refresh([FromBody]RefreshRequest? requestBody)
    {
        if (requestBody is null)
        {
            return BadRequest("Invalid Refresh request");
        }

        AuthenticatedResponse principal;
        try
        {
            principal = await _authentication.Refresh(requestBody.Token!, requestBody.RefreshToken!);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
        
        return Ok(principal);
    }

    /// <summary>
    /// [Unauthenticated user] Endpoint for signup account
    /// </summary>
    /// <param name="signupRequest">Object contains signup information</param>
    /// <returns>IActionResult</returns>
    /// <exception cref="Exception">Account is empty data</exception>
    /// <exception cref="Exception">Error if input is empty</exception>
    /// <response code="201">Returns the created account</response>
    /// <response code="400">Returns if account input is empty or create error</response>
    [HttpPost]
    [Route("signup")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GetAccountDetail), StatusCodes.Status201Created)]
    public async Task<IActionResult> Signup(
        [FromBody] SignupRequest signupRequest)
    {
        try
        {
            if (signupRequest == null)
            {
                throw new Exception("Account is empty data");
            }
            GetAccountDetail result = await _accountService.SignupAsync(signupRequest);
    
            return Created(string.Empty,result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new ErrorCustomMessage {Message = e.Message, Target = nameof(signupRequest)});
        }
    }

    /// <summary>
    /// [Authenticated user] Endpoint for change password for account
    /// </summary>
    /// <param name="bodyDto">Object contains change password information</param>
    /// <returns>IActionResult</returns>
    /// <exception cref="Exception">New password min length > 5</exception>
    /// <exception cref="Exception">Error if input is empty</exception>
    /// <response code="200">Returns an account with new password</response>
    /// <response code="400">Returns if new password input is empty or invalid</response>
    /// <response code="401">Returns if unauthorized</response>
    [HttpPost]
    [Route("changepassword")]
    [Authorize]
    [ProducesResponseType(typeof(UpdateAccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangPasswordDto bodyDto)
    {
        try
        {
            var accountId = int.Parse(User.FindFirst("id")?.Value!);
            if (bodyDto.Password.Length < 6)
                throw new MException(StatusCodes.Status400BadRequest, "Password min length 6 character");
            UpdateAccountDto result = await _accountService.ChangePasswordAsync(accountId, bodyDto.Password);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new ErrorCustomMessage {Message = e.Message, Target = nameof(ChangePassword)});
        }
    }

    /// <summary>
    /// [Authenticated User] Check correct password
    /// </summary>
    /// <param name="password">Password of account</param>
    /// <returns>result boolean</returns>
    /// <exception cref="MException">Not Valid Password</exception>
    [HttpPost("checkPassword")]
    [Authorize]
    public async Task<IActionResult> CheckPassword([FromBody]string password)
    {
        try
        {
            var accountId = int.Parse(User.FindFirst("id")?.Value!);
            if (password.Length < 6)
                throw new MException(StatusCodes.Status400BadRequest, "Password min length 6 character");
            bool result = await _accountService.CheckCorrectPassword(accountId, password);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new ErrorCustomMessage {Message = e.Message, Target = nameof(ChangePassword)});
        }
    }

    /// <summary>
    /// [Unauthenticated user] Endpoint for reset password of account
    /// </summary>
    /// <param name="phoneNumber">Phone number for reset password</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Returns the SMS message info account</response>
    /// <response code="400">Returns if account is not exist</response>
    [HttpPost]
    [Route("resetpassword")]
    [ProducesResponseType(typeof(MessagingSenderId), StatusCodes.Status200OK)]
    public async Task<IActionResult> SendSms([FromBody]string phoneNumber)
    {
        try
        {
            var result = await _authentication.ResetPassword(phoneNumber.ConvertVNPhoneNumber());
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
}