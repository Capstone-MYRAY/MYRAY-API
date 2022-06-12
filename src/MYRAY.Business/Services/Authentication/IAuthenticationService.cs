using System.Security.Claims;
using MYRAY.Business.DTOs.Account;
using MYRAY.Business.DTOs.Authentication;
using Telnyx;

namespace MYRAY.Business.Services.Authentication;
/// <summary>
/// Interface for authentication service.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Get async an token by phone number
    /// </summary>
    /// <param name="phoneNumber">Phone number of account</param>
    /// <param name="password">Password of account</param>
    /// <returns>An Object contains token and refresh token</returns>
    public Task<AuthenticatedResponse> LoginByPhoneAsync(string phoneNumber, string password);

    /// <summary>
    /// Refresh token
    /// </summary>
    /// <param name="accessToken">Access Token</param>
    /// <param name="refreshToken">Refresh Token</param>
    /// <returns>New Access token and refresh token</returns>
    public Task<AuthenticatedResponse> Refresh(string accessToken, string refreshToken);

    
    public Task Revoke(string phoneNumber);

    public Task<MessagingSenderId> ResetPassword(string phoneNumber);
}