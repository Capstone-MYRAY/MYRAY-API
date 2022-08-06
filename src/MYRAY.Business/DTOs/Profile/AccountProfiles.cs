using AutoMapper;
using MYRAY.Business.DTOs.Account;
using MYRAY.Business.DTOs.Area;
using MYRAY.Business.DTOs.Authentication;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.DTOs.Profile;

public static class AccountProfiles
{
    public static void ConfigAccountModule(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<DataTier.Entities.Account, GetAccountDetail>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.Account, InsertAccountDto>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.Account, UpdateAccountDto>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.Account, SignupRequest>().ReverseMap();
        configuration.CreateMap<InsertAccountDto, SignupRequest>().ReverseMap();

        configuration.CreateMap<AreaAccount, AreaAccountDetail>()
            .ForMember(des => des.Address, otp => otp.MapFrom(src =>
                $"{src.Area.Commune} - {src.Area.District} - {src.Area.District}"));
    }
}