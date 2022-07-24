using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Report;
using MYRAY.Business.Enums;

namespace MYRAY.Business.Services.Report;

public interface IReportService
{
    ResponseDto.CollectiveResponse<ReportDetail> GetReport(
        SearchReport searchReport,
        PagingDto pagingDto,
        SortingDto<ReportEnum.ReportSortCriterial> sortingDto);
    
    Task<ReportDetail> GetReportById(int id);
    Task<ReportDetail> GetOneReportById(int jobPostId, int reportedId, int createById);
    Task<ReportDetail> CreateReport(CreateReport report, int createBy);
    Task<ReportDetail> UpdateReport(UpdateReport report, int createBy);
    Task<ReportDetail> ResolvedReport(ResolvedReport resolvedReport, int resolveBy);
    Task<ReportDetail> DeleteReport(int id);
}