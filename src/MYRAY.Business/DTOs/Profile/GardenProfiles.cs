using AutoMapper;
using MYRAY.Business.DTOs.Garden;

namespace MYRAY.Business.DTOs.Profile;

public static class GardenProfiles
{
    public static void ConfigGardenModule(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<DataTier.Entities.Garden, GardenDetail>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.Garden, CreateGarden>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.Garden, UpdateGarden>().ReverseMap();
    }
}