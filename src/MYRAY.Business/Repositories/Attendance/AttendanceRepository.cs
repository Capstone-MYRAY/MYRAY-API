using Microsoft.EntityFrameworkCore;
using MYRAY.Business.Enums;
using MYRAY.Business.Repositories.Interface;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.Attendance;

public class AttendanceRepository : IAttendanceRepository
{
    private readonly IDbContextFactory _contextFactory;
    private readonly IBaseRepository<DataTier.Entities.Attendance> _attendanceRepository;
    private readonly IBaseRepository<DataTier.Entities.Account> _accountRepository;

    public AttendanceRepository(IDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
        _attendanceRepository =
            _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.Attendance>()!;
        _accountRepository =
            _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.Account>()!;

    }


    public async Task<DataTier.Entities.Attendance> CreateAttendance(DataTier.Entities.Attendance attendance)
    {
        // attendance.Date = DateTime.Today;
        await _attendanceRepository.InsertAsync(attendance);
        DataTier.Entities.Account? account = await _accountRepository.GetByIdAsync(attendance.AccountId);
        account.Point += attendance.BonusPoint;
        await _contextFactory.SaveAllAsync();
        return attendance;
    }

    public async Task<DataTier.Entities.Attendance?> GetAttendance(int appliedJobId, int accountId, DateTime dateTime)
    {
        DataTier.Entities.Attendance attendance = await 
            _attendanceRepository.GetFirstOrDefaultAsync(a => a.AppliedJobId == appliedJobId 
                                                              && a.AccountId == accountId 
                                                              && a.Date.Value.Date.Equals(dateTime));
        return attendance;
    }

    public async Task<double?> GetTotalExpense(int jobPostId)
    {
        IQueryable<DataTier.Entities.Attendance> query = _attendanceRepository.Get(a =>
            a.AppliedJob.JobPostId == jobPostId
            && a.Date.Value <= DateTime.Today);
        double? totalExpense = await query.SumAsync(a => a.Salary);
        return totalExpense;
    }

    public async Task<DataTier.Entities.Attendance> GetAttendanceByDate(int appliedJobId, int accountId, DateTime dateTime)
    {
        DataTier.Entities.Attendance attendance = await 
            _attendanceRepository.GetFirstOrDefaultAsync(a => a.Date.Value.Date.Equals(dateTime.Date));
        return attendance;
    }
    
    

    public IQueryable<DataTier.Entities.Attendance> GetListDayOffByJob(int farmerId, int? jobPostId = null)
    {
        IQueryable<DataTier.Entities.Attendance> query = _attendanceRepository.Get(a => a.AppliedJob.AppliedBy == farmerId
            && a.Status == (int?)AttendanceEnum.AttendanceStatus.DayOff
            && (jobPostId == null || a.AppliedJob.JobPostId == jobPostId))
            .OrderByDescending(m => m.Date.Value);
        return query;
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