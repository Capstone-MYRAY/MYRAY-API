using MYRAY.Business.DTOs.Area;

namespace MYRAY.Business.DTOs.Account;

public class AreaAccountDetail
{
    public DateTime? CreatedDate { get; set; }
    public string Address { get; set; } = null!;
    
    public string? Province { get; set; }
    public string? District { get; set; }
    public string? Commune { get; set; }
}