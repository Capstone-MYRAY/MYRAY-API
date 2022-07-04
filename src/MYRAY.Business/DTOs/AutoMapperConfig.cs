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
            md.AllowNullDestinationValues = true;
            md.AllowNullCollections = true;
            md.ConfigRoleModule();
            md.ConfigAreaModule();
            md.ConfigAccountModule();
            md.ConfigTreeTypeModule();
            md.ConfigGuidepost();
            md.ConfigJobPost();
            md.ConfigPostType();
            md.ConfigPaymentHistory();
            md.ConfigGardenModule();
            md.ConfigCommentModule();
            md.ConfigReportModule();
            md.ConfigFeedbackModule();
            md.ConfigBookmarkModule();
            md.ConfigAttendanceModule();
            md.ConfigTreeJob();
            md.ConfigMessageModule();
            md.ConfigExtendTaskModule();
        });
        IMapper mapper = mappingConfig.CreateMapper();
        services.AddSingleton(mapper);
        return services;
    }
}