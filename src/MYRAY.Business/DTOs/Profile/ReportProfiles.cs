using AutoMapper;
using MYRAY.Business.DTOs.Report;

namespace MYRAY.Business.DTOs.Profile;

public static class ReportProfiles
{
    public static void ConfigReportModule(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<DataTier.Entities.Report, ReportDetail>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.Report, CreateReport>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.Report, UpdateReport>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.Report, ResolvedReport>().ReverseMap();
    }
}