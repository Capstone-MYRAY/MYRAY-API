using AutoMapper;
using MYRAY.Business.DTOs.ExtendTaskJob;

namespace MYRAY.Business.DTOs.Profile;

public static class ExtendTaskProfiles
{
    public static void ConfigExtendTaskModule(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<DataTier.Entities.ExtendTaskJob, ExtendTaskJobDetail>()
            .ForMember(des => des.JobTitle, opt => 
                opt.MapFrom(src => src.JobPost.Title))
            .ForMember(des => des.AvatarRequest, opt => opt.MapFrom(src=>src.RequestByNavigation.ImageUrl))
            .ForMember(des => des.PhoneRequest, opt => opt.MapFrom(src=>src.RequestByNavigation.PhoneNumber))
            .ForMember(des => des.FullnameRequest, opt => opt.MapFrom(src=>src.RequestByNavigation.Fullname))
            
            .ReverseMap();
        configuration.CreateMap<DataTier.Entities.ExtendTaskJob, CreateExtendRequest>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.ExtendTaskJob, UpdateExtendRequest>().ReverseMap();
        // configuration.CreateMap<DataTier.Entities.ExtendTaskJob, ExtendTaskJobDetail>().ReverseMap();
    }
}