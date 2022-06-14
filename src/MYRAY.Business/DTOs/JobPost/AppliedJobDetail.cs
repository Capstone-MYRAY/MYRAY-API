using MYRAY.Business.DTOs.Account;

namespace MYRAY.Business.DTOs.JobPost;

public class AppliedJobDetail
{
    public int Id { get; set; }
    
    public GetAccountDetail AppliedByNavigation { get; set; } = null!;
}