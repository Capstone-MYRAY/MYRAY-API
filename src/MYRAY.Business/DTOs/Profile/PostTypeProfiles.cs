using AutoMapper;
using MYRAY.Business.DTOs.PostType;

namespace MYRAY.Business.DTOs.Profile;

public static class PostTypeProfiles
{
    public static void ConfigPostType(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<DataTier.Entities.PostType, PostTypeDetail>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.PostType, CreatePostType>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.PostType, UpdatePostType>().ReverseMap();
    }
}