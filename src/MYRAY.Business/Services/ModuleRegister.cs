using Microsoft.Extensions.DependencyInjection;

namespace MYRAY.Business.Services;

public static class ModuleRegister
{
    /// <summary>
    /// DI Service classes for Service module.
    /// </summary>
    /// <param name="services">Service container form Startup</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection RegisterServiceModule(this IServiceCollection services)
    {
        
            
        return services;
    }
}