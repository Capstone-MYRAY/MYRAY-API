using Microsoft.AspNetCore.Mvc;
using MYRAY.Api.Constants;
using MYRAY.Business.Services.JobPost;

namespace MYRAY.Api.Controllers;
/// <summary>
/// Handle request related to Cron Job.
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes(MediaType.ApplicationJson)]
[Produces(MediaType.ApplicationJson)]
public class TriggerController : ControllerBase
{
    private readonly IJobPostService _jobPostService;

    /// <summary>
    /// Initializes a new instance of the <see cref="TriggerController"/> class.
    /// </summary>
    /// <param name="jobPostService">Injection of <see cref="IJobPostService"/></param>
    public TriggerController(IJobPostService jobPostService)
    {
        _jobPostService = jobPostService;
    }

    
    /// <summary>
    /// Execute Posting Job Cron
    /// </summary>
    /// <returns>Ok With Executed</returns>
    [HttpGet("postingJob")]
    public async Task<IActionResult> PostingJobCron()
    {
        await _jobPostService.PostingJob();
        return Ok();
    }
    
    /// <summary>
    /// Execute Starting Job Cron
    /// </summary>
    /// <returns>Ok With Executed</returns>
    [HttpGet("startingJob")]
    public async Task<IActionResult> StartingJobCron()
    {
        await _jobPostService.StartingJob();
        return Ok();
    }
    
    /// <summary>
    /// Execute Expiring Job Cron
    /// </summary>
    /// <returns>Ok With Executed</returns>
    [HttpGet("expiringJob")]
    public async Task<IActionResult> ExpiringJobCron()
    {
        await _jobPostService.ExpiringJob();
        return Ok();
    }
    
    /// <summary>
    /// Execute Out Of Date Job Cron
    /// </summary>
    /// <returns>Ok With Executed</returns>
    [HttpGet("outOfDateJob")]
    public async Task<IActionResult> OutOfDateJobCron()
    {
        await _jobPostService.OutOfDateJob();
        return Ok();
    }
}