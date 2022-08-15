using MYRAY.Business.DTOs.Account;

namespace MYRAY.Business.DTOs.SalaryTracking;

public class SalaryTrackingByJob
{
    public int JobPostId { get; set; }
    public virtual GetAccountDetail Account { get; set; } = null!;
    public int? Status { get; set; }
    public virtual ICollection<SalaryTrackingDetail> Attendances { get; set; }
    
    public DateTime? EndDate { get; set; }
}