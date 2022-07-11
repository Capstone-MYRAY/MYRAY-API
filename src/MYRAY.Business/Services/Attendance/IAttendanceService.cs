using MYRAY.Business.DTOs.Attendance;

namespace MYRAY.Business.Services.Attendance;

public interface IAttendanceService
{
    
    Task CreateAttendance(CheckAttendance attendance, int checkBy, int accountId);
    Task<AttendanceDetail> CreateDayOff(RequestDayOff requestDayOff, int accountId);
    Task<List<AttendanceDetail>> GetAttendances(int jobPostId, int accountId);
}