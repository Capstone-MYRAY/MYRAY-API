namespace MYRAY.Business.Repositories.Attendance;

public interface IAttendanceRepository
{
    Task<DataTier.Entities.Attendance> CreateAttendance(DataTier.Entities.Attendance attendance);
    Task<DataTier.Entities.Attendance> GetAttendance(int appliedJobId, int accountId);
    Task<double?> GetTotalExpense(int jobPostId);
    Task<DataTier.Entities.Attendance> GetAttendanceByDate(int appliedJobId, int accountId, DateTime dateTime);
    IQueryable<MYRAY.DataTier.Entities.Attendance> GetListDayOff(int farmerId, int? jobPostId = null);
    IQueryable<DataTier.Entities.Attendance> GetListAttendances(int applyJobId);

}