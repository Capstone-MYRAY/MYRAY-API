using MYRAY.Business.DTOs.Attendance;

namespace MYRAY.Business.Services.Attendance;

public interface IAttendanceService
{
    Task CreateAttendance();
    Task CreateAttendanceManual(int jobPostId);
    Task<AttendanceDetail> CheckAttendance();
}