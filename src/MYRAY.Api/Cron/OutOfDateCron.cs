using MYRAY.Business.Services.JobPost;
using Quartz;

namespace MYRAY.Api.Cron;

public class OutOfDateCron : IJob
{
    private readonly IJobPostService _jobPostService;

    /// <summary>
    /// Initialized of instance <see cref="OutOfDateCron"/>
    /// </summary>
    /// <param name="jobPostService">Injection of <see cref="IJobPostService"/></param>
    public OutOfDateCron(IJobPostService jobPostService)
    {
        _jobPostService = jobPostService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("--OutOfDate Job Post");
        await _jobPostService.OutOfDateJob();
    }
}