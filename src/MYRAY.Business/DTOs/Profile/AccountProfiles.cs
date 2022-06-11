using AutoMapper;
using MYRAY.Business.DTOs.Account;

namespace MYRAY.Business.DTOs.Profile;

public static class AccountProfiles
{
    public static void ConfigAccountModule(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<DataTier.Entities.Account, GetAccountDetail>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.Account, InsertAccountDto>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.Account, UpdateAccountDto>().ReverseMap();

    }
}