using Microsoft.Extensions.DependencyInjection;
using MYRAY.Business.Services.Account;
using MYRAY.Business.Services.Area;
using MYRAY.Business.Services.Role;

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
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IAreaService, AreaService>();
        services.AddScoped<IAccountService, AccountService>();
        return services;
    }
}