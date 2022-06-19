using System.ComponentModel.DataAnnotations;

namespace MYRAY.Business.DTOs.Account;

public class InsertAccountDto
{
    public int RoleId { get; set; }
    
    [Required(ErrorMessage = "Fullname is required")]
    public string Fullname { get; set; } = null!;
    public string? ImageUrl { get; set; }
    
    [Required(ErrorMessage = "Date of birth is required")]
    public DateTime DateOfBirth { get; set; }
    public int Gender { get; set; }
    public string? Address { get; set; }
    
    [Required(ErrorMessage = "Phone number is required")]
    public string PhoneNumber { get; set; } = null!;
    public string? Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = null!;
    public double Balance { get; set; }
    public int Point { get; set; }
    public string? AboutMe { get; set; }
    
    public int? AreaId { get; set; }
}