using Microsoft.Extensions.DependencyInjection;
using MYRAY.Business.Repositories.Interface;


namespace MYRAY.Business.Repositories;

public static class ModuleRegister
{
    public static IServiceCollection RegisterRepositoryModule(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        services.AddScoped<IDbContextFactory, DbContextFactory>();
        //
        
        

        return services;
    }
}