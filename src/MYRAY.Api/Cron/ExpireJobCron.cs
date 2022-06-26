using MYRAY.Business.Services.JobPost;
using Quartz;

namespace MYRAY.Api.Cron;

public class ExpireJobCron : IJob
{
    private readonly IJobPostService _jobPostService;

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