using MYRAY.Business.Services.JobPost;
using Quartz;

namespace MYRAY.Api.Cron;

public class StartJobCron : IJob
{
    private readonly IJobPostService _jobPostService;
    /// <summary>
    /// Initialized of instance <see cref="StartJobCron"/>
    /// </summary>
    /// <param name="jobPostService">Injection of <see cref="IJobPostService"/></param>
    public StartJobCron(IJobPostService jobPostService)
    {
        _jobPostService = jobPostService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("--Starting Job Post");
        await _jobPostService.StartingJob();
    }
}