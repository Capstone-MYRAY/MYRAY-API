using System.Linq.Expressions;
using MYRAY.Business.Enums;
using MYRAY.Business.Repositories.Interface;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.Report;

public class ReportRepository : IReportRepository
{
    private readonly IDbContextFactory _contextFactory;
    private readonly IBaseRepository<DataTier.Entities.Report> _reportRepository;

    public ReportRepository(IDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
        _reportRepository = _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.Report>()!;
    }


    public IQueryable<DataTier.Entities.Report> GetReports()
    {
        IQueryable<DataTier.Entities.Report> query = _reportRepository.Get(r => r.Status != (int?)ReportEnum.ReportStatus.Deleted);
        return query;
    }

    public async Task<DataTier.Entities.Report> GetReportById(int id)
    {
        DataTier.Entities.Report report =
       (await _reportRepository.GetFirstOrDefaultAsync(r => r.Id == id && r.Status != (int?)ReportEnum.ReportStatus.Deleted))!;
        return report;
    }

    public async Task<DataTier.Entities.Report?> GetOneReportById(int jobPostId, int reportedId, int createById)
    {
        Expression<Func<DataTier.Entities.Report, object>> expResolved = report1 => report1.ResolvedByNavigation; 
        DataTier.Entities.Report? report =
            await _reportRepository.GetFirstOrDefaultAsync(r => r.JobPostId == jobPostId
                                                                && r.ReportedId == reportedId
                                                                && r.CreatedBy == createById, new []{expResolved});
        return report;
    }

    public async Task<DataTier.Entities.Report> CreateReport(DataTier.Entities.Report report)
    {
        await _reportRepository.InsertAsync(report);

        await _contextFactory.SaveAllAsync();

        return report;
    }

    public async Task<DataTier.Entities.Report> UpdateReport(DataTier.Entities.Report report)
    {
        
        _reportRepository.Modify(report);

        await _contextFactory.SaveAllAsync();
        return report;
    }

    public async Task<DataTier.Entities.Report> DeleteReport(int id)
    {
        DataTier.Entities.Report report = _reportRepository.GetById(id)!;
        if (report == null || report.Status == (int?)ReportEnum.ReportStatus.Deleted)
        {
            throw new Exception("Report is not existed");
        }

        report.Status = (int?)ReportEnum.ReportStatus.Deleted;
        _reportRepository.Modify(report);
        

        await _contextFactory.SaveAllAsync();

        return report;
    }
}