using AutoMapper;
using MYRAY.Business.DTOs.Message;

namespace MYRAY.Business.DTOs.Profile;

public static class MessageProfiles
{
    public static void ConfigMessageModule(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<DataTier.Entities.Message, NewMessageRequest>().ReverseMap();
    }
}