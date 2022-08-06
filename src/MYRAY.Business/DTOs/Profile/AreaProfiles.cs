using AutoMapper;
using MYRAY.Business.DTOs.Area;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.DTOs.Profile;

public static class AreaProfiles
{
    public static void ConfigAreaModule(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<DataTier.Entities.Area, GetAreaDetail>()
            .ForMember(des => des.ManagerOf, otp => otp.MapFrom(src => src.AreaAccounts))
            .ReverseMap();
        configuration.CreateMap<DataTier.Entities.Area, InsertAreaDto>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.Area, UpdateAreaDto>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.Area, DeleteAreaDto>().ReverseMap();

        configuration.CreateMap<AreaAccount, AccountManagerArea>()
            .ForMember(des => des.Fullname,
                opt => opt.MapFrom(src => src.Account.Fullname))
            .ReverseMap();
    }
}