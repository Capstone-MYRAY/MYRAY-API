using AutoMapper;
using MYRAY.Business.DTOs.Role;

namespace MYRAY.Business.DTOs.Profile;

public static class RoleProfiles
{
    public static void ConfigRoleModule(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<DataTier.Entities.Role, GetRoleDetail>().ReverseMap();
    }
}