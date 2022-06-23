using MYRAY.Business.Repositories.Interface;

namespace MYRAY.Business.Repositories.Attendance;

public class AttendanceRepository : IAttendanceRepository
{
    private readonly IDbContextFactory _contextFactory;
    private readonly IBaseRepository<DataTier.Entities.Attendance> _attendanceRepository;

    public AttendanceRepository(IDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }


    public async Task<DataTier.Entities.Attendance> CreateAttendance(DataTier.Entities.Attendance attendance)
    {
        await _attendanceRepository.InsertAsync(attendance);
        return attendance;
    }

    public async Task<DataTier.Entities.Attendance> CheckAttendance(DataTier.Entities.Attendance attendance)
    {   
        _attendanceRepository.Modify(attendance);

        await _contextFactory.SaveAllAsync();

        return attendance;
    }

    public async Task<DataTier.Entities.Attendance> RemoveAttendance(int id)
    {
        DataTier.Entities.Attendance attendance = (await _attendanceRepository.GetByIdAsync(id))!;
        
        _attendanceRepository.Delete(attendance);

        await _contextFactory.SaveAllAsync();
        return attendance;
    }
}