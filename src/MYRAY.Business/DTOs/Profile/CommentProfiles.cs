using AutoMapper;
using MYRAY.Business.DTOs.Comment;

namespace MYRAY.Business.DTOs.Profile;

public static class CommentProfiles
{
    public static void ConfigCommentModule(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<DataTier.Entities.Comment, CommentDetail>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.Comment, CreateComment>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.Comment, UpdateComment>().ReverseMap();
    }
}