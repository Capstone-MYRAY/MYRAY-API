using AutoMapper;
using MYRAY.Business.DTOs.Report;

namespace MYRAY.Business.DTOs.Profile;

public static class ReportProfiles
{
    public static void ConfigReportModule(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<DataTier.Entities.Report, ReportDetail>()
            .ForMember(des => des.RoleIdCreated, opt => opt.MapFrom(src=>src.CreatedByNavigation.Role.Name))
            .ForMember(des => des.RoleIdReported, opt => opt.MapFrom(src => src.Reported.Role.Name))
            .ReverseMap();
        configuration.CreateMap<DataTier.Entities.Report, CreateReport>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.Report, UpdateReport>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.Report, ResolvedReport>().ReverseMap();
    }
}