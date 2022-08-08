using MYRAY.Business.DTOs.Account;

namespace MYRAY.Business.DTOs.Area;

public class AccountManagerArea
{
    public DateTime? CreatedDate { get; set; }
    public int AccountId { get; set; }
    public string Fullname { get; set; } = null!;
    public string PhoneNumber { get; set; }
}