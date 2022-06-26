namespace MYRAY.Business.Repositories.Attendance;

public interface IAttendanceRepository
{
    Task<DataTier.Entities.Attendance> CreateAttendance(DataTier.Entities.Attendance attendance);
    Task<DataTier.Entities.Attendance> GetAttendance(int appliedJobId, int accountId);

    IQueryable<DataTier.Entities.Attendance> GetListAttendances(int applyJobId);

}