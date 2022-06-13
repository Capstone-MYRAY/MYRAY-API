using AutoMapper;
using MYRAY.Business.DTOs.TreeType;

namespace MYRAY.Business.DTOs.Profile;

public static class TreeTypeProfiles
{
    public static void ConfigTreeTypeModule(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<DataTier.Entities.TreeType, CreateTreeType>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.TreeType, UpdateTreeType>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.TreeType, TreeTypeDetail>().ReverseMap();
    }
}