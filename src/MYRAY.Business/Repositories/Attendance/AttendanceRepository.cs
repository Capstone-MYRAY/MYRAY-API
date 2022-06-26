using MYRAY.Business.Repositories.Interface;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.Attendance;

public class AttendanceRepository : IAttendanceRepository
{
    private readonly IDbContextFactory _contextFactory;
    private readonly IBaseRepository<DataTier.Entities.Attendance> _attendanceRepository;

    public AttendanceRepository(IDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
        _attendanceRepository =
            _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.Attendance>()!;
    }


    public async Task<DataTier.Entities.Attendance> CreateAttendance(DataTier.Entities.Attendance attendance)
    {
        attendance.Date = DateTime.Today;
        await _attendanceRepository.InsertAsync(attendance);
        await _contextFactory.SaveAllAsync();
        return attendance;
    }

    public async Task<DataTier.Entities.Attendance> GetAttendance(int appliedJobId, int accountId)
    {
        DataTier.Entities.Attendance attendance = await 
            _attendanceRepository.GetFirstOrDefaultAsync(a => a.Date.Value.Date.Equals(DateTime.Today));
        return attendance;
    }

    public IQueryable<DataTier.Entities.Attendance> GetListAttendances(int applyJobId)
    {
        IQueryable<DataTier.Entities.Attendance> query = _attendanceRepository.Get(a => a.AppliedJobId == applyJobId);
        return query;
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