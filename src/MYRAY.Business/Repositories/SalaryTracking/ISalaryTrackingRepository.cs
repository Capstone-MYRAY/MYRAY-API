namespace MYRAY.Business.Repositories.SalaryTracking;

public interface ISalaryTrackingRepository
{
    Task<DataTier.Entities.SalaryTracking> CreateSalaryTracking(DataTier.Entities.SalaryTracking attendance);
    Task<DataTier.Entities.SalaryTracking?> GetSalaryTracking(int appliedJobId, int accountId, DateTime dateTime);
    Task<double?> GetTotalExpense(int jobPostId);
    Task<DataTier.Entities.SalaryTracking> GetSalaryTrackingByDate(int appliedJobId, int accountId, DateTime dateTime);
    IQueryable<MYRAY.DataTier.Entities.SalaryTracking> GetListDayOffByJob(int farmerId, int? jobPostId = null);
    IQueryable<DataTier.Entities.SalaryTracking> GetListSalaryTrackings(int applyJobId);

}