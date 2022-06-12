using System.ComponentModel.DataAnnotations;

namespace MYRAY.Business.DTOs.Authentication;

public class SignupRequest
{
    [Required(ErrorMessage = "Role is required")]
    public int RoleId { get; set; }
    
    [Required(ErrorMessage = "Fullname is required")]
    public string Fullname { get; set; } = null!;
    
    [Required(ErrorMessage = "PhoneNumber is required")]
    public string PhoneNumber { get; set; } = null!;
    
    [Required(ErrorMessage = "Date of birth is required")]
    public DateTime DateOfBirth { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = null!;
}