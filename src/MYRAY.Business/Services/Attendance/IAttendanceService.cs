using MYRAY.Business.DTOs.Attendance;
using MYRAY.Business.DTOs.Attendance;
using MYRAY.Business.Enums;

namespace MYRAY.Business.Services.Attendance;

public interface IAttendanceService
{
    // public ResponseDto.CollectiveResponse<AttendanceByJob> GetAttendanceByJob(PagingDto pagingDto);
    Task<AttendanceDetail> CreateAttendance(CheckAttendance attendance, int checkBy);
    Task<AttendanceDetail> CreateDayOff(RequestDayOff requestDayOff, int accountId);
    Task<List<AttendanceDetail>> GetListDayOffByJob(int farmerId, int? jobPostId);
    Task<List<AttendanceDetail>> GetAttendances(int jobPostId, int accountId);
    Task<double?> GetTotalExpense(int jobPostId);

    Task<List<AttendanceByJob?>> GetAttendanceByDate(int jobPostId, DateTime dateTime ,AttendanceEnum.AttendanceStatus? status = null);
}