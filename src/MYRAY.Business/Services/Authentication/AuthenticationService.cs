using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MYRAY.Business.Constants;
using MYRAY.Business.DTOs.Account;
using MYRAY.Business.DTOs.Authentication;
using MYRAY.Business.Enums;
using MYRAY.Business.Exceptions;
using MYRAY.Business.Helpers;
using MYRAY.Business.Repositories.Account;
using Telnyx;

namespace MYRAY.Business.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IMapper _mapper;
    private readonly IAccountRepository _accountRepository;
    private readonly IConfiguration _configuration;

    public AuthenticationService(IMapper mapper, IAccountRepository accountRepository, IConfiguration configuration)
    {
        _mapper = mapper;
        _accountRepository = accountRepository;
        _configuration = configuration;
    }

    public async Task<AuthenticatedResponse> LoginByPhoneAsync(string phoneNumber, string password)
    {
        if (!phoneNumber.IsValidPhoneNumber())
        {
            throw new MException(StatusCodes.Status401Unauthorized, "Invalid Phone Number", phoneNumber);
        }
        DataTier.Entities.Account queryAccount = await _accountRepository.GetAccountByPhoneAsync(phoneNumber);

        if (queryAccount == null)
        {
            throw new MException(StatusCodes.Status401Unauthorized, "Invalid Login Information", nameof(phoneNumber));
        }

        if (!queryAccount.Password.Equals(password))
        {
            throw new MException(StatusCodes.Status401Unauthorized, "Invalid Password");
        }

        if (queryAccount.Status == (int?)AccountEnum.AccountStatus.Banned)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Account has been locked");
        }

        string role = "";

        switch (queryAccount.RoleId)
        {
            case 1: role = UserRole.ADMIN;
                break;
            case 2: role = UserRole.MODERATOR;
                break;
            case 3: role = UserRole.LANDOWNER;
                break;
            case 4: role = UserRole.FARMER;
                break;
        }


        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.Name, queryAccount.PhoneNumber),
            new Claim("id", queryAccount.Id.ToString()),
            new Claim("FullName", queryAccount.Fullname)
        };

        string accessToken = GenerateAccessToken(claims);
        string refreshToken = GenerateRefreshToken();
        
        queryAccount.RefreshToken = refreshToken;
        
        var refreshToValidity = _configuration
            .GetSection("JWT")
            .GetSection("RefreshTokenValidityInDays").Value;
        queryAccount.RefreshTokenExpiryTime = DateTime.Now.AddDays(int.Parse(refreshToValidity));

        await _accountRepository.UpdateAccountAsync(queryAccount, null);

        return new AuthenticatedResponse
        {
            Token = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthenticatedResponse> Refresh(string accessToken, string refreshToken)
    {
        var principal = GetPrincipalFromExpiredToken(accessToken);
        var phoneNumber = principal.Identity!.Name;

        DataTier.Entities.Account? account = await _accountRepository.GetAccountByPhoneAsync(phoneNumber!);

        if (account is null || !account.RefreshToken!.Equals(refreshToken) ||
            account.RefreshTokenExpiryTime <= DateTime.Now)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Invalid Request Refresh Token is invalid or expired");
        }

        string newAccessToken = GenerateAccessToken(principal.Claims);
        string newRefreshToken = GenerateRefreshToken();

        account.RefreshToken = newRefreshToken;
        await _accountRepository.UpdateAccountAsync(account, null);

        return new AuthenticatedResponse
        {
            Token = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    public async Task Revoke(string phoneNumber)
    {
        DataTier.Entities.Account account = await _accountRepository.GetAccountByPhoneAsync(phoneNumber);
        if (account == null) throw new MException(StatusCodes.Status400BadRequest, "Account is not exist");

        account.RefreshToken = null;

        await _accountRepository.UpdateAccountAsync(account, null);
    }

    private string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secret = Encoding.ASCII.GetBytes(_configuration
            .GetSection("JWT")
            .GetSection("Secret").Value);
        var timeToValidity = _configuration
            .GetSection("JWT")
            .GetSection("TokenValidityInMinutes").Value;
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(int.Parse(timeToValidity)),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        return jwtToken;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secret = Encoding.ASCII.GetBytes(_configuration
            .GetSection("JWT")
            .GetSection("Secret").Value);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secret),
            ValidateLifetime = false
        };
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature,
                StringComparison.InvariantCultureIgnoreCase)) ;
        return principal;
    }
    
    public async Task<MessagingSenderId> ResetPassword(string phoneNumber)
    {
        try
        {
            if (!phoneNumber.IsValidPhoneNumber())
            {
                throw new MException(StatusCodes.Status401Unauthorized, "Invalid Phone Number", phoneNumber);
            }
            
            DataTier.Entities.Account account = await _accountRepository.GetAccountByPhoneAsync(
                phoneNumber);
            account.Password = CreatePassword(6);
            
            account = await _accountRepository.UpdateAccountAsync(account, null);
            string msg = $"[MYRAY] Mật khẩu mới của bạn là: {account.Password}";
            var result = await SendSMSHelper.SendSMSTelnyx(account.PhoneNumber, msg, _configuration);
            Console.WriteLine("Send SMS to: " + account.PhoneNumber);
            UpdateAccountDto updateAccountDto = _mapper.Map<UpdateAccountDto>(account);
            return result;
        }
        catch (Exception e)
        {
            throw new MException(StatusCodes.Status400BadRequest, e.Message, nameof(e.TargetSite.Name));
        }
    }
    
    private string CreatePassword(int length)
    {
        const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        StringBuilder res = new StringBuilder();
        Random rnd = new Random();
        while (0 < length--)
        {
            res.Append(valid[rnd.Next(valid.Length)]);
        }
        return res.ToString();
    }
}