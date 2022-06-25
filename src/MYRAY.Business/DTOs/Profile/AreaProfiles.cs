using AutoMapper;
using MYRAY.Business.DTOs.Area;
using MYRAY.Business.DTOs.Role;

namespace MYRAY.Business.DTOs.Profile;

public static class AreaProfiles
{
    public static void ConfigAreaModule(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<DataTier.Entities.Area, GetAreaDetail>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.Area, InsertAreaDto>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.Area, UpdateAreaDto>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.Area, DeleteAreaDto>().ReverseMap();
    }
}