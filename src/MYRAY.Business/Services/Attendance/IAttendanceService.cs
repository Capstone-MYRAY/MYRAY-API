using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Attendance;

namespace MYRAY.Business.Services.Attendance;

public interface IAttendanceService
{
    // public ResponseDto.CollectiveResponse<AttendanceByJob> GetAttendanceByJob(PagingDto pagingDto);
    Task CreateAttendance(CheckAttendance attendance, int checkBy, int accountId);
    Task<AttendanceDetail> CreateDayOff(RequestDayOff requestDayOff, int accountId);
    Task<List<AttendanceDetail>> GetListDayOff(int farmerId, int? jobPostId);
    Task<List<AttendanceDetail>> GetAttendances(int jobPostId, int accountId);
    Task<double?> GetTotalExpense(int jobPostId);
}