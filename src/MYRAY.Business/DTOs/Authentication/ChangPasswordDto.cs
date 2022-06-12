using System.ComponentModel.DataAnnotations;

namespace MYRAY.Business.DTOs.Authentication;

public class ChangPasswordDto
{
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = null!;
}