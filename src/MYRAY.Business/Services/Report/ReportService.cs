using AutoMapper;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Report;
using MYRAY.Business.Enums;
using MYRAY.Business.Helpers;
using MYRAY.Business.Helpers.Paging;
using MYRAY.Business.Repositories.Report;

namespace MYRAY.Business.Services.Report;

public class ReportService : IReportService
{
    private readonly IMapper _mapper;
    private readonly IReportRepository _reportRepository;

    public ReportService(IMapper mapper, IReportRepository reportRepository)
    {
        _mapper = mapper;
        _reportRepository = reportRepository;
    }


    public ResponseDto.CollectiveResponse<ReportDetail> GetReport(SearchReport searchReport, PagingDto pagingDto, SortingDto<ReportEnum.ReportSortCriterial> sortingDto)
    {
        IQueryable<DataTier.Entities.Report> query = _reportRepository.GetReports();

        query = query.GetWithSearch(searchReport);

        query = query.GetWithSorting(sortingDto.SortColumn.ToString(), sortingDto.OrderBy);

        var result = query.GetWithPaging<ReportDetail, DataTier.Entities.Report>(pagingDto, _mapper);

        return result;
    }

    public async Task<ReportDetail> GetReportById(int id)
    {
        DataTier.Entities.Report report = await _reportRepository.GetReportById(id);
        ReportDetail result = _mapper.Map<ReportDetail>(report);
        return result;
    }

    public async Task<ReportDetail> GetOneReportById(int jobPostId, int reportedId, int createById)
    {
        DataTier.Entities.Report? report = await _reportRepository.GetOneReportById(jobPostId, reportedId, createById);
        ReportDetail result = _mapper.Map<ReportDetail>(report);
        return result;
    }

    public async Task<ReportDetail> CreateReport(CreateReport report, int createBy)
    {
        DataTier.Entities.Report reportOri = _mapper.Map<DataTier.Entities.Report>(report);
        reportOri.CreatedBy = createBy;
        reportOri.CreatedDate = DateTime.Now;
        reportOri = await _reportRepository.CreateReport(reportOri);
        ReportDetail result = _mapper.Map<ReportDetail>(reportOri);
        return result;
    }

    public async Task<ReportDetail> UpdateReport(UpdateReport report, int createBy)
    {
        DataTier.Entities.Report reportOri = _mapper.Map<DataTier.Entities.Report>(report);
        reportOri.CreatedBy = createBy;
        reportOri.CreatedDate = DateTime.Now;
        reportOri = await _reportRepository.UpdateReport(reportOri);
        ReportDetail result = _mapper.Map<ReportDetail>(reportOri);
        return result;
    }

    public async Task<ReportDetail> ResolvedReport(ResolvedReport resolvedReport, int resolveBy)
    {
        DataTier.Entities.Report reportOri = _mapper.Map<DataTier.Entities.Report>(resolvedReport);
        reportOri.ResolvedBy = resolveBy;
        reportOri.ResolvedDate = DateTime.Now;
        reportOri.CreatedBy = null;
        reportOri.Status = (int?)ReportEnum.ReportStatus.Resolved;
        reportOri = await _reportRepository.UpdateReport(reportOri);
        ReportDetail result = _mapper.Map<ReportDetail>(reportOri);
        return result;
    }

    public async Task<ReportDetail> DeleteReport(int id)
    {
         DataTier.Entities.Report report = await _reportRepository.DeleteReport(id);
         ReportDetail result = _mapper.Map<ReportDetail>(report);
         return result;
    }
}