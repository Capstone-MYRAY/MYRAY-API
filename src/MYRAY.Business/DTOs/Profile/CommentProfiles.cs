using AutoMapper;
using MYRAY.Business.DTOs.Comment;

namespace MYRAY.Business.DTOs.Profile;

public static class CommentProfiles
{
    public static void ConfigCommentModule(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<DataTier.Entities.Comment, CommentDetail>()
            .ForMember(des => des.Fullname, map => map.MapFrom(src => src.CommentByNavigation.Fullname))
            .ForMember(des => des.Avatar, map => map.MapFrom(src => src.CommentByNavigation.ImageUrl))
            .ReverseMap();
        configuration.CreateMap<DataTier.Entities.Comment, CreateComment>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.Comment, UpdateComment>().ReverseMap();
    }
}