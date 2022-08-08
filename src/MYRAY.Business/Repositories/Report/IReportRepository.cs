namespace MYRAY.Business.Repositories.Report;

public interface IReportRepository
{
    IQueryable<DataTier.Entities.Report> GetReports();

    IQueryable<MYRAY.DataTier.Entities.Report> GetReportsByReportedId(int reportedId);
    Task<DataTier.Entities.Report> GetReportById(int id);
    Task<DataTier.Entities.Report?> GetOneReportById(int jobPostId, int reportedId, int createById);
    Task<DataTier.Entities.Report> CreateReport(DataTier.Entities.Report report);
    Task<DataTier.Entities.Report> UpdateReport(DataTier.Entities.Report report);
    Task<DataTier.Entities.Report> DeleteReport(int id);
}