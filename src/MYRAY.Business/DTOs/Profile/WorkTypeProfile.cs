using AutoMapper;
using MYRAY.Business.DTOs.PostType;
using MYRAY.Business.DTOs.WorkType;

namespace MYRAY.Business.DTOs.Profile;

public static class WorkTypeProfile
{
    public static void ConfigWorkType(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<DataTier.Entities.WorkType, WorkTypeDetail>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.WorkType, CreateWorkType>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.WorkType, UpdateWorkType>().ReverseMap();
    }
}