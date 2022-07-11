namespace MYRAY.Business.Repositories.Attendance;

public interface IAttendanceRepository
{
    Task<DataTier.Entities.Attendance> CreateAttendance(DataTier.Entities.Attendance attendance);
    Task<DataTier.Entities.Attendance> GetAttendance(int appliedJobId, int accountId);
    Task<DataTier.Entities.Attendance> GetAttendanceByDate(int appliedJobId, int accountId, DateTime dateTime);

    IQueryable<DataTier.Entities.Attendance> GetListAttendances(int applyJobId);

}