using Microsoft.Extensions.DependencyInjection;
using MYRAY.Business.Services.Account;
using MYRAY.Business.Services.AppliedJob;
using MYRAY.Business.Services.Area;
using MYRAY.Business.Services.Authentication;
using MYRAY.Business.Services.Comment;
using MYRAY.Business.Services.Garden;
using MYRAY.Business.Services.Guidepost;
using MYRAY.Business.Services.JobPost;
using MYRAY.Business.Services.PostType;
using MYRAY.Business.Services.Role;
using MYRAY.Business.Services.TreeType;

namespace MYRAY.Business.Services;

public static class ModuleRegister
{
    /// <summary>
    /// DI Service classes for Service module.
    /// </summary>
    /// <param name="services">Service container form Startup</param>
    /// <returns>IServiceCollection</returns>
    public static void RegisterServiceModule(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IAreaService, AreaService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ITreeTypeService, TreeTypeService>();
        services.AddScoped<IGuidepostService, GuidepostService>();
        services.AddScoped<IJobPostService, JobPostService>();
        services.AddScoped<IAppliedJobService, AppliedJobService>();
        services.AddScoped<IPostTypeService, PostTypeService>();
        services.AddScoped<IPaymentHistoryService, PaymentHistoryService>();
        services.AddScoped<IGardenService, GardenService>();
        services.AddScoped<ICommentService, CommentService>();
    }
}