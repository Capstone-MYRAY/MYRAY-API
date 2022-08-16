using System.ComponentModel.DataAnnotations;
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

    /// <summary>
    /// Initializes a new instance of the <see cref="JobPostController"/> class.
    /// </summary>
    /// <param name="jobPostService">Injection of <see cref="IJobPostService"/></param>
    /// <param name="appliedJobService">Injection of <see cref="IAppliedJobService"/></param>
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
    /// <param name="publishBy">Id of account landowner</param>
    /// <returns>List of job post</returns>
    /// <response code="200">Returns the list of job post.</response>
    /// <response code="204">Returns if list of job post is empty.</response>
    /// <response code="403">Returns if token is access denied.</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDto.CollectiveResponse<JobPostDetail>), StatusCodes.Status200OK)]
    public Task<IActionResult> GetJobPost(
        [FromQuery] SearchJobPost searchJobPost,
        [FromQuery] SortingDto<JobPostEnum.JobPostSortCriteria> sortingDto,
        [FromQuery] PagingDto pagingDto, int? publishBy = null)
    {
        var isFarmer = User.IsInRole(UserRole.FARMER);
        var result = _jobPostService.GetJobPosts(searchJobPost, pagingDto, sortingDto, publishBy, isFarmer);
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
    [ProducesResponseType(typeof(JobPostDetail), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetJobPostById(int jobPostId)
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
    [ProducesResponseType(typeof(JobPostDetail), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateJobPost(CreateJobPost createJobPost)
    {
        try
        {
            var accountId = int.Parse(User.FindFirst("id")?.Value!);
            //var accountId = 3;
            var result = await _jobPostService.CreateJobPost(createJobPost, accountId);

            return Created(string.Empty, result);
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
    [ProducesResponseType(typeof(JobPostDetail), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateJobPost(UpdateJobPost updateJobPost)
    {
        try
        {
            var accountId = int.Parse(User.FindFirst("id")?.Value!);
            //var accountId = 3;
            var result = await _jobPostService.UpdateJobPost(updateJobPost, accountId);

            return await GetJobPostById(result.Id);
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
    [ProducesResponseType(typeof(JobPostDetail), StatusCodes.Status200OK)]
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
    /// [Landowner] Endpoint for cancel job post.
    /// </summary>
    /// <param name="jobPostId">Id of job post</param>
    /// <returns>Async function</returns>
    /// <response code="204">Returns the job post cancel</response>
    /// <response code="404">Returns if job post is not existed.</response>
    /// <response code="401">Returns if invalid authorize.</response>
    [HttpDelete("cancel/{jobPostId}")]
    [Authorize(Roles = UserRole.LANDOWNER)]
    [ProducesResponseType(typeof(JobPostDetail), StatusCodes.Status200OK)]
    public async Task<IActionResult> CancelJobPost(int jobPostId)
    {
        try
        {
            var result = await _jobPostService.CancelJobPost(jobPostId);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
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
        var accountId = int.Parse(User.FindFirst("id")?.Value!);
        // var accountId = 3;
        try
        {
            var result = await _appliedJobService.ApplyJob(jobPostId, accountId);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Landowner] Endpoint for End job post
    /// </summary>
    /// <param name="jobPostId">Id of job post</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Returns if end success</response>
    /// <response code="400">Returns if job post not existed or applied</response>
    /// <response code="401">Returns if invalid authorize</response>
    [HttpPatch("end/{jobPostId}")]
    [Authorize(Roles = UserRole.LANDOWNER)]
    public async Task<IActionResult> EndJob(int jobPostId)
    {
        // var accountId = int.Parse(User.FindFirst("id")?.Value!);
        try
        {
            var result = await _jobPostService.EndJobPost(jobPostId);

            return Ok(result);
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

            return Ok(result);
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
    /// [Landowner] Endpoint for get all apply 
    /// </summary>
    /// <param name="status">Status of applied</param>
    /// <param name="pagingDto">An object contains paging criteria.</param>
    /// <param name="sortingDto">An object contains sorting criteria</param>
    /// <returns>List of account with id applied</returns>
    /// <response code="200">Returns the list of account with id applied.</response>
    /// <response code="204">Returns if list of account is empty.</response>
    /// <response code="401">Returns if token is access denied.</response>
    [HttpGet("allApplied")]
    [Authorize(Roles = UserRole.LANDOWNER)]
    [ProducesResponseType(typeof(ResponseDto.CollectiveResponse<AppliedJobDetail>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> GetAllAppliedLandowner(
        [FromQuery] AppliedJobEnum.AppliedJobStatus? status,
        [FromQuery] PagingDto pagingDto,
        [FromQuery] SortingDto<AppliedJobEnum.SortCriteriaAppliedJob> sortingDto)
    {
        var accountId = int.Parse(User.FindFirst("id")?.Value!);
        ResponseDto.CollectiveResponse<AppliedJobDetail> result =
            _appliedJobService.GetAllAccountsApplied(pagingDto, sortingDto, accountId, status);
        if (result == null)
        {
            return Task.FromResult<IActionResult>(NoContent());
        }

        return Task.FromResult<IActionResult>(Ok(result));
    }

    /// <summary>
    /// [Authenticated User] Endpoint for get all account apply to job post
    /// </summary>
    /// <param name="status">Status of applied</param>
    /// <param name="jobPostId">Id of job post</param>
    /// <param name="pagingDto">An object contains paging criteria.</param>
    /// <param name="sortingDto">An object contains sorting criteria</param>
    /// <returns>List of account with id applied</returns>
    /// <response code="200">Returns the list of account with id applied.</response>
    /// <response code="204">Returns if list of account is empty.</response>
    /// <response code="401">Returns if token is access denied.</response>
    [HttpGet("applied")]
    [Authorize(Roles = UserRole.LANDOWNER + "," + UserRole.FARMER)]
    [ProducesResponseType(typeof(ResponseDto.CollectiveResponse<AppliedJobDetail>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> GetAppliedLandowner(
        [FromQuery] AppliedJobEnum.AppliedJobStatus? status,
        [Required] int jobPostId,
        [FromQuery] PagingDto pagingDto,
        [FromQuery] SortingDto<AppliedJobEnum.SortCriteriaAppliedJob> sortingDto)
    {
        ResponseDto.CollectiveResponse<AppliedJobDetail> result =
            _appliedJobService.GetAccountsApplied(pagingDto, sortingDto, jobPostId, status);
        if (result == null)
        {
            return Task.FromResult<IActionResult>(NoContent());
        }

        return Task.FromResult<IActionResult>(Ok(result));
    }

    /// <summary>
    /// [Farmer] Endpoint for get all job farmer applied 
    /// </summary>
    /// <param name="status">Status of applied</param>
    /// <param name="pagingDto">An object contains paging criteria.</param>
    /// <param name="sortingDto">An object contains sorting criteria.</param>
    /// <param name="startWork">Status work of job post</param>
    /// <returns>List of account with id applied</returns>
    /// <response code="200">Returns the list of account with id applied.</response>
    /// <response code="204">Returns if list of account is empty.</response>
    /// <response code="401">Returns if token is access denied.</response>
    [HttpGet("appliedFarmer")]
    [Authorize(Roles = UserRole.FARMER)]
    [ProducesResponseType(typeof(ResponseDto.CollectiveResponse<AppliedJobDetail>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAccounts(
        [FromQuery] AppliedJobEnum.AppliedJobStatus? status,
        [FromQuery] PagingDto pagingDto,
        [FromQuery] SortingDto<AppliedJobEnum.SortCriteriaAppliedJob> sortingDto,
        [FromQuery] int? startWork = null)
    {
        var accountId = int.Parse(User.FindFirst("id")?.Value!);
        ResponseDto.CollectiveResponse<AppliedJobDetail> result =
            _appliedJobService.GetAccountsAppliedFarmer(pagingDto, sortingDto, accountId, status, startWork);
        if (result == null)
        {
            return NoContent();
        }

        return Ok(result);
    }

    /// <summary>
    /// [Farmer] Endpoint for check farmer apply to specific job post
    /// </summary>
    /// <param name="jobPostId">Id of job post</param>
    /// <returns>List of account with id applied</returns>
    /// <response code="200">Returns if the farmer does not applied to job post.</response>
    /// <response code="201">Returns if the farmer has been applied to job post.</response>
    /// <response code="401">Returns if token is access denied.</response>
    [HttpGet("checkApplied")]
    [Authorize(Roles = UserRole.FARMER)]
    public async Task<IActionResult> CheckAppliedJob(int jobPostId)
    {
        var accountId = int.Parse(User.FindFirst("id")?.Value!);
        AppliedJobDetail? appliedJob = await _appliedJobService.CheckApplied(jobPostId, accountId);
        if (appliedJob == null)
        {
            return Ok();
        }

        return Created(String.Empty, null);
    }

    /// <summary>
    /// [Farmer] Endpoint for check farmer apply to hour job post
    /// </summary>
    /// <returns>True if has applied hour job</returns>
    /// <response code="200">Returns result farmer applied hour job.</response>
    /// <response code="401">Returns if token is access denied.</response>
    [HttpGet("checkAppliedHourJob")]
    [Authorize(Roles = UserRole.FARMER)]
    public async Task<IActionResult> CheckAppliedHourJob()
    {
        var accountId = int.Parse(User.FindFirst("id")?.Value!);
        bool result = await _appliedJobService.CheckAppliedHourJob(accountId);
        return Ok(result);
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
    [ProducesResponseType(typeof(JobPostDetail), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ApproveJobPost(int jobPostId)
    {
        try
        {
            var accountId = int.Parse(User.FindFirst("id")?.Value!);
            var result = await _jobPostService.ApproveJobPost(jobPostId, accountId);

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
    /// <param name="rejectJobPost">Contain id and reason to reject job post</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Returns if reject success</response>
    /// <response code="400">Returns if job post not existed or posted</response>
    /// <response code="401">Returns if invalid authorize</response>
    [HttpPatch("rejectJob")]
    [Authorize(Roles = UserRole.MODERATOR)]
    [ProducesResponseType(typeof(JobPostDetail), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RejectJobPost(RejectJobPost rejectJobPost)
    {
        try
        {
            var accountId = int.Parse(User.FindFirst("id")?.Value!);
            var result = await _jobPostService.RejectJobPost(rejectJobPost, accountId);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Landowner] Endpoint for Start job post
    /// </summary>
    /// <param name="jobPostId">Id of job post</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Returns if start success</response>
    /// <response code="400">Returns if job post not existed or started</response>
    /// <response code="401">Returns if invalid authorize</response>
    [HttpPatch("startJob/{jobPostId}")]
    [Authorize(Roles = UserRole.LANDOWNER)]
    [ProducesResponseType(typeof(JobPostDetail), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> StartJobPost(int jobPostId)
    {
        try
        {
            var result = await _jobPostService.StartJobPost(jobPostId);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Landowner] Endpoint for Extend end date job post
    /// </summary>
    /// <param name="jobPostId">Id of job post</param>
    /// <param name="extendDate">New end date for job post</param>
    /// <param name="usePoint">Point use to extend</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Returns if extend success</response>
    /// <response code="400">Returns if job post not existed or posted</response>
    /// <response code="401">Returns if invalid authorize</response>
    [HttpPatch("extendJob/{jobPostId}")]
    [Authorize(Roles = UserRole.LANDOWNER)]
    [ProducesResponseType(typeof(JobPostDetail), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ExtendJobPost([Required] int jobPostId, [Required] DateTime extendDate,
        int usePoint = 0)
    {
        try
        {
            var result = await _jobPostService.ExtendJobPostForLandowner(jobPostId, extendDate, usePoint);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }


    /// <summary>
    /// [Landowner] Endpoint to check available pin date
    /// </summary>
    /// <param name="publishedDate">Publish Date</param>
    /// <param name="numberOfDayPublish">Number of publish day</param>
    /// <param name="postTypeId">Id of post type</param>
    /// <returns>List of datetime available</returns>
    [HttpGet("checkPinDate")]
    [Authorize(Roles = UserRole.LANDOWNER)]
    [ProducesResponseType(typeof(IEnumerable<DateTime>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckPinDateAvailable(
        [Required] DateTime publishedDate,
        [Required] int numberOfDayPublish,
        [Required] int postTypeId)
    {
        try
        {
            var result = await _jobPostService.ListDateNoPin(publishedDate, numberOfDayPublish, postTypeId);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }


    /// <summary>
    /// [Landowner] Endpoint to get max pin day
    /// </summary>
    /// <param name="pinDate">Pin date</param>
    /// <param name="numberPublishDay">Number of publish day</param>
    /// <param name="postTypeId">Id of post type</param>
    /// <returns>Number of max pin date</returns>
    [HttpGet("getMaxPinDay")]
    [Authorize(Roles = UserRole.LANDOWNER)]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckNumberOfPinDate(
        DateTime pinDate,
        int numberPublishDay,
        int postTypeId)
    {
        try
        {
            var result = await _jobPostService.MaxNumberOfPinDate(pinDate, numberPublishDay, postTypeId);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Authenticated User] Endpoint to get total pin day of job post
    /// </summary>
    /// <param name="jobPostId">Id of job post</param>
    /// <returns>Number pin day of job post</returns>
    [HttpGet("totalPinDay/{jobPostId}")]
    [Authorize]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> TotalPinDay([Required] int jobPostId)
    {
        try
        {
            int result = await _jobPostService.TotalPinDate(jobPostId);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Landowner] Endpoint for extend max farmer for hour job
    /// </summary>
    /// <param name="jobPostId">Id Of job post.</param>
    /// <param name="maxFarmer">Max farmer.</param>
    [HttpPatch("extendFarmer")]
    [Authorize(Roles = UserRole.LANDOWNER)]
    public async Task<IActionResult> ExtendMaxFarmer(
        [Required] int jobPostId,
        [Required] int maxFarmer)
    {
        try
        {
            await _jobPostService.ExtendMaxFarmer(jobPostId, maxFarmer);
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e);
        }
    }

    /// <summary>
    /// [Landowner] Endpoint for switch status of job post shorthanded <-> enough.
    /// </summary>
    /// <param name="jobPostId">Id of job post.</param>
    [HttpPatch("switch/{jobPostId}")]
    [Authorize(Roles = UserRole.LANDOWNER)]
    public async Task<IActionResult> SwitchStatus([Required] int jobPostId)
    {
        try
        {
            await _jobPostService.SwitchStatusJob(jobPostId);
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e);
        }
    }

    /// <summary>
    /// [Farmer] Endpoint for count applied pending or approve.
    /// </summary>
    /// <returns>Number of request applied</returns>
    [HttpGet("countApplied")]
    [Authorize(Roles = UserRole.FARMER)]
    public async Task<IActionResult> CountApplied()
    {
        var accountId = int.Parse(User.FindFirst("id")?.Value!);
        var isFarmer = User.IsInRole(UserRole.FARMER);
        if (!isFarmer)
        {
            return BadRequest("Available Farmer Only");
        }
        try
        {
           
            int result = await _appliedJobService.AlreadyApplied(accountId);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            BadRequest(e);
        }

        return Ok(0);
    }
}