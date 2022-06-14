using AutoMapper;
using MYRAY.Business.DTOs.Guidepost;

namespace MYRAY.Business.DTOs.Profile;

public static class GuidepostProfiles
{
    public static void ConfigGuidepost(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<DataTier.Entities.Guidepost, GuidepostDetail>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.Guidepost, CreateGuidepost>().ReverseMap();
            
        configuration.CreateMap<DataTier.Entities.Guidepost, UpdateGuidepost>()
            .ForAllMembers(otp => otp.Condition(src => src != null));

        configuration.CreateMap<UpdateGuidepost, DataTier.Entities.Guidepost>();


    }
}