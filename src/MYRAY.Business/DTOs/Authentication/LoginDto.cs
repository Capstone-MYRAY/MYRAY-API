using System.ComponentModel.DataAnnotations;

namespace MYRAY.Business.DTOs.Authentication;

public class LoginDto
{
    [Required(ErrorMessage = "Phone Number is required")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}