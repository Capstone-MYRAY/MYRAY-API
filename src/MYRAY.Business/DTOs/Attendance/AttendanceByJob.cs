using MYRAY.Business.DTOs.Account;

namespace MYRAY.Business.DTOs.Attendance;

public class AttendanceByJob
{
    public int JobPostId { get; set; }
    public virtual GetAccountDetail Account { get; set; } = null!;
    public virtual ICollection<AttendanceDetail> Attendances { get; set; }
}