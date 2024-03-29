using Microsoft.Extensions.DependencyInjection;
using MYRAY.Business.Repositories.Account;
using MYRAY.Business.Repositories.AppliedJob;
using MYRAY.Business.Repositories.Area;
using MYRAY.Business.Repositories.AreaAccount;
using MYRAY.Business.Repositories.Attendance;
using MYRAY.Business.Repositories.Bookmark;
using MYRAY.Business.Repositories.Comment;
using MYRAY.Business.Repositories.Config;
using MYRAY.Business.Repositories.ExtendTaskJob;
using MYRAY.Business.Repositories.Feedback;
using MYRAY.Business.Repositories.Garden;
using MYRAY.Business.Repositories.Guidepost;
using MYRAY.Business.Repositories.Interface;
using MYRAY.Business.Repositories.JobPost;
using MYRAY.Business.Repositories.Message;
using MYRAY.Business.Repositories.PaymentHistory;
using MYRAY.Business.Repositories.PostType;
using MYRAY.Business.Repositories.Report;
using MYRAY.Business.Repositories.Role;
using MYRAY.Business.Repositories.Statistic;
using MYRAY.Business.Repositories.TreeJob;
using MYRAY.Business.Repositories.TreeType;
using MYRAY.Business.Repositories.WorkType;


namespace MYRAY.Business.Repositories;

public static class ModuleRegister
{
    public static IServiceCollection RegisterRepositoryModule(this IServiceCollection services)
    {
        // Register Base 
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        services.AddScoped<IDbContextFactory, DbContextFactory>();
        // Register Data Repositories
        services.AddTransient<IRoleRepository, RoleRepository>();
        services.AddTransient<IAreaRepository, AreaRepository>();
        services.AddTransient<IAccountRepository, AccountRepository>();
        services.AddTransient<ITreeTypeRepository, TreeTypeRepository>();
        services.AddTransient<IGuidepostRepository, GuidepostRepository>();
        services.AddTransient<IJobPostRepository, JobPostRepository>();
        services.AddTransient<IPaymentHistoryRepository, PaymentHistoryRepository>();
        services.AddTransient<IAppliedJobRepository, AppliedJobRepository>();
        services.AddTransient<IPaymentHistoryRepository, PaymentHistoryRepository>();
        services.AddTransient<IPostTypeRepository, PostTypeRepository>();
        services.AddTransient<IGardenRepository, GardenRepository>();
        services.AddTransient<ICommentRepository, CommentRepository>();
        services.AddTransient<IReportRepository, ReportRepository>();
        services.AddTransient<IAreaAccountRepository, AreaAccountRepository>();
        services.AddTransient<IFeedbackRepository, FeedbackRepository>();
        services.AddTransient<IBookmarkRepository, BookmarkRepository>();
        services.AddTransient<IAttendanceRepository, AttendanceRepository>();
        services.AddTransient<IMessageRepository, MessageRepository>();
        services.AddTransient<ITreeJobRepository, TreeJobRepository>();
        services.AddTransient<IExtendTaskJobRepository, ExtendTaskJobRepository>();
        services.AddTransient<IStatisticRepository, StatisticRepository>();
        services.AddTransient<IConfigRepository, ConfigRepository>();
        services.AddTransient<IWorkTypeRepository, WorkTypeRepository>();
        
        return services;
    }
}