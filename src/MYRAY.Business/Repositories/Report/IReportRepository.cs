namespace MYRAY.Business.Repositories.Report;

public interface IReportRepository
{
    IQueryable<DataTier.Entities.Report> GetReports();
    Task<DataTier.Entities.Report> GetReportById(int id);
    Task<DataTier.Entities.Report> CreateReport(DataTier.Entities.Report report);
    Task<DataTier.Entities.Report> UpdateReport(DataTier.Entities.Report report);
    Task<DataTier.Entities.Report> DeleteReport(int id);
}