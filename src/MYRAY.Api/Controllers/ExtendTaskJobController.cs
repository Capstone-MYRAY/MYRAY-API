using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYRAY.Api.Constants;
using MYRAY.Business.Constants;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.ExtendTaskJob;
using MYRAY.Business.Enums;
using MYRAY.Business.Services.ExtendTaskJob;

namespace MYRAY.Api.Controllers;

/// <summary>
/// Handle request related to Account
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes(MediaType.ApplicationJson)]
[Produces(MediaType.ApplicationJson)]
public class ExtendTaskJobController : ControllerBase
{
    private readonly IExtendTaskJobService _extendTaskJobService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExtendTaskJobController"/> class.
    /// </summary>
    /// <param name="extendTaskJobService">Injection of <see cref="IExtendTaskJobService"/></param>

    public ExtendTaskJobController(IExtendTaskJobService extendTaskJobService)
    {
        _extendTaskJobService = extendTaskJobService;
    }
    
    /// <summary>
    /// [Landowner, Farmer] Endpoint for get all request extend task job of farmer with condition
    /// </summary>
    /// <param name="searchJobPost">An object contains filter criteria.</param>
    /// <param name="sortingDto">An object contains sorting criteria.</param>
    /// <param name="pagingDto">An object contains paging criteria.</param>
    /// <returns>List of request extend</returns>
    /// <response code="200">Returns the list of request extend.</response>
    /// <response code="204">Returns if list of request extend is empty.</response>
    /// <response code="403">Returns if token is access denied.</response>
    [HttpGet]
    [Authorize(Roles = UserRole.LANDOWNER + "," + UserRole.FARMER)]
    [ProducesResponseType(typeof(ResponseDto.CollectiveResponse<ExtendTaskJobDetail>),StatusCodes.Status200OK)]
    public Task<IActionResult> GetExtendTaskJobByFarmer(
        [FromQuery] SearchExtendRequest searchJobPost,
        [FromQuery] SortingDto<ExtendTaskJobEnum.SortCriteriaExtendTaskJob> sortingDto,
        [FromQuery] PagingDto pagingDto)
    {
        int? accountId = null;
        if (User.Claims.First().Value.Equals("Landowner"))
        {
            accountId = int.Parse(User.FindFirst("id")!.Value);
        }
        var result =  _extendTaskJobService.GetExtendTaskJobsAll(searchJobPost, pagingDto, sortingDto, accountId);
        if (result == null)
        {
            return Task.FromResult<IActionResult>(NoContent());
        }

        return Task.FromResult<IActionResult>(Ok(result));
    }

    /// <summary>
    /// [Farmer] Endpoint to check job post extend task existed.
    /// </summary>
    /// <param name="jobPostId">Id of job post</param>
    /// <returns>Return if extend job is existed</returns>
    [HttpGet("{jobPostId}")]
    [Authorize(Roles = UserRole.FARMER+ "," + UserRole.LANDOWNER)]
    [ProducesResponseType(typeof(ExtendTaskJobDetail),StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExtendTaskJobByJobId(int jobPostId)
    {
        var result = await  _extendTaskJobService.CheckOneExtend(jobPostId);
        if (result == null)
        {
            return NoContent();
        }

        return Ok(result);
    }

    /// <summary>
    /// [Farmer] Endpoint for create request extend task
    /// </summary>
    /// <param name="createExtendRequest">Object contains extend request dto</param>
    /// <returns>IActionResult</returns>
    /// <exception cref="Exception">Extend request is empty data</exception>
    /// <exception cref="Exception">Error if input is empty</exception>
    /// <response code="201">Returns the created request</response>
    /// <response code="400">Returns if request input is empty or create error</response>
    /// <response code="401">Returns if invalid authorize</response>
    [HttpPost]
    [Authorize(Roles = UserRole.FARMER)]
    [ProducesResponseType(typeof(ExtendTaskJobDetail),StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateExtendTaskJob(CreateExtendRequest createExtendRequest)
    {
        try
        {
            var requestId = int.Parse(User.FindFirst("id")?.Value!);
            //var requestId = 3;
            var result = await _extendTaskJobService.CreateExtendTaskJob(createExtendRequest, requestId);

            return Created(string.Empty, result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// [Farmer] Endpoint for update extend request
    /// </summary>
    /// <param name="updateExtendRequest">An object contains update information</param>
    /// <returns>An job post updated</returns>
    /// <response code="200">Returns the extend request updated</response>
    /// <response code="400">Returns if input extend request information empty</response>
    /// <response code="401">Returns if invalid authorize.</response>
    [HttpPut]
    [Authorize(Roles = UserRole.FARMER)]
    [ProducesResponseType(typeof(ExtendTaskJobDetail),StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateExtendRequest(UpdateExtendRequest updateExtendRequest)
    {
        try
        {
            var requestId = int.Parse(User.FindFirst("id")?.Value!);
            //var requestId = 3;
            var result = await _extendTaskJobService.UpdateExtendTaskJob(updateExtendRequest, requestId);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// [Farmer] Endpoint for delete request extend.
    /// </summary>
    /// <param name="extendTaskJobId">Id of request extend</param>
    /// <returns>Async function</returns>
    /// <response code="204">Returns the request extend deleted</response>
    /// <response code="404">Returns if request extend is not existed.</response>
    /// <response code="401">Returns if invalid authorize.</response>
    [HttpDelete("{extendTaskJobId}")]
    [Authorize(Roles = UserRole.FARMER)]
    [ProducesResponseType(typeof(ExtendTaskJobDetail),StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteExtendTaskJob(int extendTaskJobId)
    {
        try
        {
            var result = await _extendTaskJobService.DeleteExtendTaskJob(extendTaskJobId);

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// [Landowner] Endpoint for Approve request extend
    /// </summary>
    /// <param name="extendTaskJobId">Id of extend request</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Returns if apply success</response>
    /// <response code="400">Returns if extend request not existed or approved</response>
    /// <response code="401">Returns if invalid authorize</response>
    [HttpPatch("approve/{extendTaskJobId}")]
    [Authorize(Roles = UserRole.LANDOWNER)]
    public async Task<IActionResult> ApproveExtend(int extendTaskJobId)
    {
        // var approvedId = 3;
        try
        {
            var approvedId = int.Parse(User.FindFirst("id")?.Value!);
            var result = await _extendTaskJobService.ApproveExtendTaskJob(extendTaskJobId, approvedId);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// [Landowner] Endpoint for Reject request extend
    /// </summary>
    /// <param name="extendTaskJobid">Id of extend request</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Returns if reject success</response>
    /// <response code="400">Returns if request extend not existed or approved</response>
    /// <response code="401">Returns if invalid authorize</response>
    [HttpPatch("reject/{extendTaskJobid}")]
    [Authorize(Roles = UserRole.LANDOWNER)]
    public async Task<IActionResult> RejectExtend(int extendTaskJobid)
    {
        try
        {
            var approvedId = int.Parse(User.FindFirst("id")?.Value!);
            var result = await _extendTaskJobService.RejectExtendTaskJob(extendTaskJobid, approvedId);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
    
    
}