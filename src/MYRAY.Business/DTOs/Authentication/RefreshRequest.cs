using System.ComponentModel.DataAnnotations;

namespace MYRAY.Business.DTOs.Authentication;

public class RefreshRequest
{
    [Required(ErrorMessage = "Token is required")]
    public string? Token { get; set; }
    
    [Required(ErrorMessage = "Refresh token is required")]
    public string? RefreshToken { get; set; }
}