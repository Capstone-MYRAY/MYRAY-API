using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYRAY.Api.Constants;
using MYRAY.Business.Constants;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.JobPost;
using MYRAY.Business.Enums;
using MYRAY.Business.Services.AppliedJob;
using MYRAY.Business.Services.JobPost;

namespace MYRAY.Api.Controllers;
/// <summary>
/// Handle request related to jobpost
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes(MediaType.ApplicationJson)]
[Produces(MediaType.ApplicationJson)]
public class JobPostController : ControllerBase
{
    private readonly IJobPostService _jobPostService;
    private readonly IAppliedJobService _appliedJobService;

    public JobPostController(IJobPostService jobPostService, IAppliedJobService appliedJobService)
    {
        _jobPostService = jobPostService;
        _appliedJobService = appliedJobService;
    }

    /// <summary>
    /// [Authenticated user] Endpoint for get all job post with condition
    /// </summary>
    /// <param name="searchJobPost">An object contains filter criteria.</param>
    /// <param name="sortingDto">An object contains sorting criteria.</param>
    /// <param name="pagingDto">An object contains paging criteria.</param>
    /// <returns>List of job post</returns>
    /// <response code="200">Returns the list of job post.</response>
    /// <response code="204">Returns if list of job post is empty.</response>
    /// <response code="403">Returns if token is access denied.</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDto.CollectiveResponse<JobPostDetail>),StatusCodes.Status200OK)]
    public Task<IActionResult> GetJobPost(
        [FromQuery] SearchJobPost searchJobPost,
        [FromQuery] SortingDto<JobPostEnum.JobPostSortCriteria> sortingDto,
        [FromQuery] PagingDto pagingDto, int? publishBy = null)
    {
        var result =  _jobPostService.GetJobPosts(searchJobPost, pagingDto, sortingDto, publishBy);
        if (result == null)
        {
            return Task.FromResult<IActionResult>(NoContent());
        }

        return Task.FromResult<IActionResult>(Ok(result));
    }
    
    /// <summary>
    /// [Authenticated user] Endpoint for get job post information by Identifier.
    /// </summary>
    /// <param name="jobPostId">An id of job post</param>
    /// <returns>A job post</returns>
    /// <response code="200">Returns the job post.</response>
    /// <response code="400">Returns if job post is not existed.</response>
    /// <response code="401">Returns if not authorize</response>
    [HttpGet("{jobPostId}")]
    [Authorize]
    [ProducesResponseType(typeof(JobPostDetail),StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGuidepostById(int jobPostId)
    {
        try
        {
            var result = await _jobPostService.GetJobPostById(jobPostId);
            
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// [Landowner] Endpoint for create job post
    /// </summary>
    /// <param name="createJobPost">Object contains job post dto</param>
    /// <returns>IActionResult</returns>
    /// <exception cref="Exception">job post is empty data</exception>
    /// <exception cref="Exception">Error if input is empty</exception>
    /// <response code="201">Returns the created job post</response>
    /// <response code="400">Returns if job post input is empty or create error</response>
    /// <response code="401">Returns if invalid authorize</response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(JobPostDetail),StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateJobPost(CreateJobPost createJobPost)
    {
        try
        {
            var accountId = int.Parse(User.FindFirst("id")?.Value!);
            //var accountId = 3;
            var result = await _jobPostService.CreateJobPost(createJobPost, accountId);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// [Landowner] Endpoint for update job post
    /// </summary>
    /// <param name="updateJobPost">An object contains update information</param>
    /// <returns>An job post updated</returns>
    /// <response code="200">Returns the job post updated</response>
    /// <response code="400">Returns if input job post information empty</response>
    /// <response code="401">Returns if invalid authorize.</response>
    [HttpPut]
    [Authorize(Roles = UserRole.LANDOWNER)]
    [ProducesResponseType(typeof(JobPostDetail),StatusCodes.Status201Created)]
    public async Task<IActionResult> UpdateJobPost(UpdateJobPost updateJobPost)
    {
        try
        {
            var accountId = int.Parse(User.FindFirst("id")?.Value!);
            //var accountId = 3;
            var result = await _jobPostService.UpdateJobPost(updateJobPost, accountId);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// [Landowner] Endpoint for delete job post.
    /// </summary>
    /// <param name="jobPostId">Id of job post</param>
    /// <returns>Async function</returns>
    /// <response code="204">Returns the job post deleted</response>
    /// <response code="404">Returns if job post is not existed.</response>
    /// <response code="401">Returns if invalid authorize.</response>
    [HttpDelete("{jobPostId}")]
    [Authorize(Roles = UserRole.LANDOWNER)]
    [ProducesResponseType(typeof(JobPostDetail),StatusCodes.Status201Created)]
    public async Task<IActionResult> DeleteJobPost(int jobPostId)
    {
        try
        {
            var result = await _jobPostService.DeleteJobPost(jobPostId);

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    
    /// <summary>
    /// [Farmer] Endpoint for Apply job post
    /// </summary>
    /// <param name="jobPostId">Id of job post</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Returns if apply success</response>
    /// <response code="400">Returns if job post not existed or applied</response>
    /// <response code="401">Returns if invalid authorize</response>
    [HttpPatch("apply/{jobPostId}")]
    [Authorize(Roles = UserRole.FARMER)]
    public async Task<IActionResult> ApplyJob(int jobPostId)
    {
        //var accountId = int.Parse(User.FindFirst("id")?.Value!);
        var accountId = 3;
        try
        {
            var result = await _appliedJobService.ApplyJob(jobPostId, accountId);

            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    
    /// <summary>
    /// [Farmer] Endpoint for Cancel apply job post
    /// </summary>
    /// <param name="jobPostId">Id of job post</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Returns if cancel success</response>
    /// <response code="400">Returns if job post not existed or applied</response>
    /// <response code="401">Returns if invalid authorize</response>
    [HttpPatch("cancel/{jobPostId}")]
    [Authorize(Roles = UserRole.FARMER)]
    public async Task<IActionResult> CancelApply(int jobPostId)
    {
        var accountId = int.Parse(User.FindFirst("id")?.Value!);
        // var accountId = 3;
        try
        {
            var result = await _appliedJobService.CancelApply(jobPostId, accountId);

            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// [Landowner] Endpoint for Approve apply job post
    /// </summary>
    /// <param name="appliedId">Id of appliedJob</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Returns if approve success</response>
    /// <response code="400">Returns if appliedJob not existed or applied</response>
    /// <response code="401">Returns if invalid authorize</response>
    [HttpPatch("approve/{appliedId}")]
    [Authorize(Roles = UserRole.LANDOWNER)]
    public async Task<IActionResult> ApproveApply(int appliedId)
    {
        try
        {
            var result = await _appliedJobService.ApproveJob(appliedId);

            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// [Landowner] Endpoint for Reject apply job post
    /// </summary>
    /// <param name="appliedId">Id of appliedJob</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Returns if reject success</response>
    /// <response code="400">Returns if appliedJob not existed or applied</response>
    /// <response code="401">Returns if invalid authorize</response>
    [HttpPatch("reject/{appliedId}")]
    [Authorize(Roles = UserRole.LANDOWNER)]
    public async Task<IActionResult> RejectApply(int appliedId)
    {
        try
        {
            var result = await _appliedJobService.RejectJob(appliedId);

            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Landowner] Endpoint for get all account apply to job post
    /// </summary>
    /// <param name="status">Status of applied</param>
    /// <param name="jobPostId">Id of job post</param>
    /// <param name="pagingDto">An object contains paging criteria.</param>
    /// <returns>List of account with id applied</returns>
    /// <response code="200">Returns the list of account with id applied.</response>
    /// <response code="204">Returns if list of account is empty.</response>
    /// <response code="401">Returns if token is access denied.</response>
    [HttpGet("applied")]
    [Authorize(Roles = UserRole.LANDOWNER)]
    [ProducesResponseType(typeof(ResponseDto.CollectiveResponse<AppliedJobDetail>),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> GetAccounts(
        [FromQuery]AppliedJobEnum.AppliedJobStatus? status,
        [FromQuery] int jobPostId,
        [FromQuery]PagingDto pagingDto)
    {
        ResponseDto.CollectiveResponse<AppliedJobDetail> result =
            _appliedJobService.GetAccountsApplied(pagingDto, jobPostId, status);
        if (result == null)
        {
            return Task.FromResult<IActionResult>(NoContent());
        }
        return Task.FromResult<IActionResult>(Ok(result));
    }
    
    /// <summary>
    /// [Moderator] Endpoint for Approve job post
    /// </summary>
    /// <param name="jobPostId">Id of job post</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Returns if approve success</response>
    /// <response code="400">Returns if job post not existed or posted</response>
    /// <response code="401">Returns if invalid authorize</response>
    [HttpPatch("approveJob/{jobPostId}")]
    [Authorize(Roles = UserRole.MODERATOR)]
    [ProducesResponseType(typeof(JobPostDetail),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ApproveJobPost(int jobPostId)
    {
        try
        {
            var result = await _jobPostService.ApproveJobPost(jobPostId);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
    
    
    /// <summary>
    /// [Moderator] Endpoint for Reject job post
    /// </summary>
    /// <param name="jobPostId">Id of job post</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Returns if reject success</response>
    /// <response code="400">Returns if job post not existed or posted</response>
    /// <response code="401">Returns if invalid authorize</response>
    [HttpPatch("rejectJob/{jobPostId}")]
    // [Authorize(Roles = UserRole.MODERATOR)]
    [ProducesResponseType(typeof(JobPostDetail),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RejectJobPost(int jobPostId)
    {
        try
        {
            var result = await _jobPostService.RejectJobPost(jobPostId);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
    
}