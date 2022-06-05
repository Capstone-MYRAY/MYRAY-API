using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MYRAY.Business.DTOs.Profile;

namespace MYRAY.Business.DTOs;

public static class AutoMapperConfig
{
    /// <summary>
    /// DI Service classes for AutoMapper module.
    /// </summary>
    /// <param name="services">Service container form Startup</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
    {
            
        var mappingConfig = new MapperConfiguration(md =>
        {
            md.ConfigRoleModule();
        });
        IMapper mapper = mappingConfig.CreateMapper();
        services.AddSingleton(mapper);
        return services;
    }
}