using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MYRAY.DataTier.Entities;

namespace MYRAY.DataTier;

/// <summary>
/// Add custom services into Dependency Injection Container.
/// </summary>
public static class ModuleRegister
{
    /// <summary>
    /// DI Service classes for DataTier module.
    /// </summary>
    /// <param name="services">Service container form Startup</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection RegisterDataTierModule(this IServiceCollection services)
    {
        services.AddScoped<DbContext, MYRAYContext>();

        return services;
    }
}