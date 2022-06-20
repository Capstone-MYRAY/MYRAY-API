using AutoMapper;
using MYRAY.Business.DTOs.Feedback;

namespace MYRAY.Business.DTOs.Profile;

public static class FeedbackProfiles
{
    public static void ConfigFeedbackModule(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<DataTier.Entities.Feedback, FeedbackDetail>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.Feedback, CreateFeedback>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.Feedback, UpdateFeedback>().ReverseMap();
    }
}