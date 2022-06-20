using AutoMapper;
using MYRAY.Business.DTOs.Bookmark;

namespace MYRAY.Business.DTOs.Profile;

public static class BookmarkProfiles
{
    public static void ConfigBookmarkModule(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<DataTier.Entities.Bookmark, BookmarkDetail>().ReverseMap();
    }
}