using MYRAY.Business.Services.JobPost;
using Quartz;

namespace MYRAY.Api.Cron;

public class PostJobCron : IJob
{
    private static readonly string ScheduleCronExpression = "* * * ? * *";
    // private static readonly string ScheduleCronExpression = "0 * * ? * *";
    // private static readonly string ScheduleCronExpression = "0 0 12 * * ?";
    private readonly IJobPostService _jobPostService;

    public PostJobCron(IJobPostService jobPostService)
    {
        _jobPostService = jobPostService;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("--Posting Job Post");
        await _jobPostService.PostingJob();
    }
}