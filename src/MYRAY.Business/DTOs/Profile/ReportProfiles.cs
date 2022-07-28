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
            .ForMember(des => des.CreatedName, opt => opt.MapFrom(src => src.CreatedByNavigation.Fullname))
            .ForMember(des => des.ReportedName, opt => opt.MapFrom(src => src.Reported.Fullname))
            .ForMember(des => des.ResolvedName, opt => opt.MapFrom(src => src.ResolvedByNavigation.Fullname))
            .ForMember(des => des.JobPostTitle, opt => opt.MapFrom(src => src.JobPost.Title))
            .ForMember(des => des.ReportedPhone, opt => opt.MapFrom(src => src.Reported.PhoneNumber))
            .ForMember(des => des.ReportedAvatar, opt => opt.MapFrom(src => src.Reported.ImageUrl))
            .ReverseMap();
        configuration.CreateMap<DataTier.Entities.Report, CreateReport>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.Report, UpdateReport>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.Report, ResolvedReport>().ReverseMap();
    }
}