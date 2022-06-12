using Microsoft.AspNetCore.Identity;

namespace MYRAY.Business.DTOs.Authentication;

public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}