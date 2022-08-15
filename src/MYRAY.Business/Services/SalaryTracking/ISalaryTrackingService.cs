using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Attendance;
using MYRAY.Business.DTOs.SalaryTracking;
using MYRAY.Business.DTOs.SalaryTracking;
using MYRAY.Business.Enums;

namespace MYRAY.Business.Services.SalaryTracking;

public interface ISalaryTrackingService
{
    // public ResponseDto.CollectiveResponse<SalaryTrackingByJob> GetSalaryTrackingByJob(PagingDto pagingDto);
    Task<SalaryTrackingDetail> CreateSalaryTracking(CheckAttendance attendance, int checkBy);
    Task<SalaryTrackingDetail> CreateDayOff(RequestDayOff requestDayOff, int accountId);
    Task<List<SalaryTrackingDetail>> GetListDayOffByJob(int farmerId, int? jobPostId);
    Task<List<SalaryTrackingDetail>> GetSalaryTrackings(int jobPostId, int accountId);
    Task<double?> GetTotalExpense(int jobPostId);

    Task<List<SalaryTrackingByJob?>> GetSalaryTrackingByDate(int jobPostId, DateTime dateTime ,SalaryTrackingEnum.SalaryTrackingStatus? status = null);
}