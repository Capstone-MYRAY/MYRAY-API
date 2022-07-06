using Quartz;

namespace MYRAY.Api.Cron;

public static class ModuleRegister
{
    [Obsolete("Obsolete")]
    public static void RegisterQuartz(this IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory(opt =>
            {
                opt.AllowDefaultConstructor = true;
            });
            var postJobPost = new JobKey("PostJobPostCron", "JobPostGroup");
            var expireJobPost = new JobKey("ExpireJobPostCron", "JobPostGroup");
            var startJobPost = new JobKey("StatJobPostCron", "JobPostGroup");
            
            q.AddJob<PostJobCron>(o => o.WithIdentity(postJobPost));
            q.AddJob<ExpireJobCron>(o => o.WithIdentity(expireJobPost));
            q.AddJob<StartJobCron>(o => o.WithIdentity(startJobPost));
            
            
            q.AddTrigger(opts => opts.ForJob(postJobPost)
                .WithIdentity("PostJobPostTrigger")
                .WithCronSchedule("0 0 0 ? * *"));

            q.AddTrigger(opts => opts.ForJob(expireJobPost)
                .WithIdentity("ExpireJobPostTrigger")
                .WithCronSchedule("0 0 0 ? * *"));
            
            q.AddTrigger(opts => opts.ForJob(startJobPost)
                .WithIdentity("StartJobPostTrigger")
                .WithCronSchedule("0 0 0 ? * *"));
            
            
            q.InterruptJobsOnShutdown = true;
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        
        services.AddTransient<IJob, PostJobCron>();
    }
}