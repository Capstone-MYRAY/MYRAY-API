using Microsoft.Extensions.DependencyInjection;
using MYRAY.Business.DTOs;
using MYRAY.Business.Repositories;
using MYRAY.Business.Services;

namespace MYRAY.Business;

/// <summary>
/// Add custom services into Dependency Injection Container.
/// </summary>
public static class ModuleRegister
{
    /// <summary>
    /// DI Service classes for Business module.
    /// </summary>
    /// <param name="services">Service container form Startup</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection RegisterBusinessModule(this IServiceCollection services)
    {
        //--Register (DI) Service Module
         services.RegisterRepositoryModule();
            
        //--Register (DI) Auto Mapper
        services.ConfigureAutoMapper();
            
        //--Register (DI) Service Module
        services.RegisterServiceModule();

        return services;
    }
}