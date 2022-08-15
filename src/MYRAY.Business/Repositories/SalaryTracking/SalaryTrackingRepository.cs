using Microsoft.EntityFrameworkCore;
using MYRAY.Business.Enums;
using MYRAY.Business.Repositories.Interface;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.SalaryTracking;

public class SalaryTrackingRepository : ISalaryTrackingRepository
{
    private readonly IDbContextFactory _contextFactory;
    private readonly IBaseRepository<DataTier.Entities.SalaryTracking> _attendanceRepository;
    private readonly IBaseRepository<DataTier.Entities.Account> _accountRepository;
    private readonly IBaseRepository<DataTier.Entities.AppliedJob> _appliedRepository;

    public SalaryTrackingRepository(IDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
        _attendanceRepository =
            _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.SalaryTracking>()!;
        _accountRepository =
            _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.Account>()!;
        _appliedRepository =
            _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.AppliedJob>()!;

    }


    public async Task<DataTier.Entities.SalaryTracking> CreateSalaryTracking(DataTier.Entities.SalaryTracking attendance)
    {
        // attendance.Date = DateTime.Today;
        await _attendanceRepository.InsertAsync(attendance);
        DataTier.Entities.Account? account = await _accountRepository.GetByIdAsync(attendance.AccountId);
        account!.Point += attendance.BonusPoint;
        DataTier.Entities.AppliedJob? appliedJob = await _appliedRepository.GetFirstOrDefaultAsync(a => a.Id == attendance.AppliedJobId);
        if (attendance.Status == (int?)SalaryTrackingEnum.SalaryTrackingStatus.Dismissed)
        {
            appliedJob!.Status = (int?)AppliedJobEnum.AppliedJobStatus.Fired;
            appliedJob.EndDate = DateTime.Today;
        }
        if (attendance.Status == (int?)SalaryTrackingEnum.SalaryTrackingStatus.End)
        {
            appliedJob!.Status = (int?)AppliedJobEnum.AppliedJobStatus.End;
            appliedJob.EndDate = DateTime.Today;
        }
        
        await _contextFactory.SaveAllAsync();
        return attendance;
    }

    public async Task<DataTier.Entities.SalaryTracking?> GetSalaryTracking(int appliedJobId, int accountId, DateTime dateTime)
    {
        DataTier.Entities.SalaryTracking attendance = await 
            _attendanceRepository.GetFirstOrDefaultAsync(a => a.AppliedJobId == appliedJobId 
                                                              && a.AccountId == accountId 
                                                              && a.Date.Value.Date.Equals(dateTime));
        return attendance;
    }

    public async Task<double?> GetTotalExpense(int jobPostId)
    {
        
        IQueryable<DataTier.Entities.SalaryTracking> query = _attendanceRepository.Get(a =>
            a.AppliedJob.JobPostId == jobPostId
            && a.Date.Value.Date <= DateTime.Today);
        double? totalExpense = await query.SumAsync(a => a.Salary);
        return totalExpense;
    }

    public async Task<DataTier.Entities.SalaryTracking> GetSalaryTrackingByDate(int appliedJobId, int accountId, DateTime dateTime)
    {
        DataTier.Entities.SalaryTracking attendance = await 
            _attendanceRepository.GetFirstOrDefaultAsync(a => a.Date.Value.Date.Equals(dateTime.Date));
        return attendance;
    }
    
    

    public IQueryable<DataTier.Entities.SalaryTracking> GetListDayOffByJob(int farmerId, int? jobPostId = null)
    {
        IQueryable<DataTier.Entities.SalaryTracking> query = _attendanceRepository.Get(a => a.AppliedJob.AppliedBy == farmerId
            && a.Status == (int?)SalaryTrackingEnum.SalaryTrackingStatus.DayOff
            && (jobPostId == null || a.AppliedJob.JobPostId == jobPostId))
            .OrderByDescending(m => m.Date.Value);
        return query;
    }

    public IQueryable<DataTier.Entities.SalaryTracking> GetListSalaryTrackings(int applyJobId)
    {
        IQueryable<DataTier.Entities.SalaryTracking> query = _attendanceRepository.Get(a => a.AppliedJobId == applyJobId);
        return query;
    }

    public async Task<DataTier.Entities.SalaryTracking> CheckSalaryTracking(DataTier.Entities.SalaryTracking attendance)
    {   
        _attendanceRepository.Modify(attendance);

        await _contextFactory.SaveAllAsync();

        return attendance;
    }

    public async Task<DataTier.Entities.SalaryTracking> RemoveSalaryTracking(int id)
    {
        DataTier.Entities.SalaryTracking attendance = (await _attendanceRepository.GetByIdAsync(id))!;
        
        _attendanceRepository.Delete(attendance);

        await _contextFactory.SaveAllAsync();
        return attendance;
    }
}