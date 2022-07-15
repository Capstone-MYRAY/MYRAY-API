using MYRAY.Business.Services.JobPost;
using Quartz;

namespace MYRAY.Api.Cron;

public class ExpireJobCron : IJob
{
    private readonly IJobPostService _jobPostService;

    /// <summary>
    /// Initialized of instance <see cref="ExpireJobCron"/>
    /// </summary>
    /// <param name="jobPostService">Injection of <see cref="IJobPostService"/></param>
    public ExpireJobCron(IJobPostService jobPostService)
    {
        _jobPostService = jobPostService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("--Expiring Job Post");
        await _jobPostService.ExpiringJob();
    }
}