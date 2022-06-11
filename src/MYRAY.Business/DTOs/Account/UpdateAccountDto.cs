namespace MYRAY.Business.DTOs.Account;

public class UpdateAccountDto
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public string Fullname { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int Gender { get; set; }
    public string? Address { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string? Email { get; set; }
    // public double Balance { get; set; }
    // public int Point { get; set; }
    public string? AboutMe { get; set; }
}