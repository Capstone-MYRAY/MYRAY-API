namespace MYRAY.Business.Repositories.Attendance;

public interface IAttendanceRepository
{
    Task<DataTier.Entities.Attendance> CreateAttendance(DataTier.Entities.Attendance attendance);
    Task<DataTier.Entities.Attendance> CheckAttendance(DataTier.Entities.Attendance attendance);
    Task<DataTier.Entities.Attendance> RemoveAttendance(int id);
}