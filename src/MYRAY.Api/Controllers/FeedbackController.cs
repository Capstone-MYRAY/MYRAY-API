using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYRAY.Api.Constants;
using MYRAY.Business.Constants;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Feedback;
using MYRAY.Business.Enums;
using MYRAY.Business.Services.Feedback;

namespace MYRAY.Api.Controllers;

/// <summary>
/// Handle request related to feedback
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes(MediaType.ApplicationJson)]
[Produces(MediaType.ApplicationJson)]
public class FeedbackController : ControllerBase
{
    private readonly IFeedbackService _feedbackService;

    public FeedbackController(IFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    /// <summary>
    /// [Authenticated user] Endpoint for get all feedback with condition
    /// </summary>
    /// <param name="searchFeedback">An object contains filter criteria.</param>
    /// <param name="sortingDto">An object contains sorting criteria.</param>
    /// <param name="pagingDto">An object contains paging criteria.</param>
    /// <returns>List of feedback</returns>
    /// <response code="200">Returns the list of feedback.</response>
    /// <response code="204">Returns if list of feedback is empty.</response>
    /// <response code="403">Returns if token is access denied.</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDto.CollectiveResponse<FeedbackDetail>), StatusCodes.Status200OK)]
    public Task<IActionResult> GetFeedback(
        [FromQuery] SearchFeedback searchFeedback,
        [FromQuery] SortingDto<FeedbackEnum.FeedbackSortCriteria> sortingDto,
        [FromQuery] PagingDto pagingDto)
    {
        var result = _feedbackService.GetFeedbacks(searchFeedback, pagingDto, sortingDto);
        if (result == null)
        {
            return Task.FromResult<IActionResult>(NoContent());
        }

        return Task.FromResult<IActionResult>(Ok(result));
    }

    /// <summary>
    /// [Authenticated user] Endpoint for get feedback information by Identifier.
    /// </summary>
    /// <param name="jobPostId">An id of job post</param>
    /// <param name="belongedId">Id of account belonged</param>
    /// <param name="createdById">Id of account createBy</param>
    /// <returns>An account</returns>
    /// <response code="200">Returns the feedback.</response>
    /// <response code="204">Returns if feedback is not existed.</response>
    /// <response code="401">Returns if not authorize</response>
    [HttpGet("one")]
    [Authorize]
    [ProducesResponseType(typeof(FeedbackDetail), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOneFeedbackById(
        [Required] int jobPostId, 
        [Required] int belongedId, 
        [Required] int createdById)
    {
        try
        {
            var result = await _feedbackService.GetOneFeedback(jobPostId, belongedId, createdById);
            if (result == null) return NoContent();
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Authenticated user] Endpoint for get a feedback information with condition.
    /// </summary>
    /// <param name="feedbackId">An id of feedback</param>
    /// <returns>An account</returns>
    /// <response code="200">Returns the feedback.</response>
    /// <response code="400">Returns if feedback is not existed.</response>
    /// <response code="401">Returns if not authorize</response>
    [HttpGet("{feedbackId}")]
    [Authorize]
    [ProducesResponseType(typeof(FeedbackDetail), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFeedbackById(int feedbackId)
    {
        try
        {
            var result = await _feedbackService.GetFeedbackById(feedbackId);

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Landowner, Farmer] Endpoint for create feedback
    /// </summary>
    /// <param name="createFeedback">Object contains feedback dto</param>
    /// <returns>IActionResult</returns>
    /// <exception cref="Exception">feedback is empty data</exception>
    /// <exception cref="Exception">Error if input is empty</exception>
    /// <response code="201">Returns the created feedback</response>
    /// <response code="400">Returns if feedback input is empty or create error</response>
    /// <response code="401">Returns if invalid authorize</response>
    [HttpPost]
    [Authorize(Roles = UserRole.LANDOWNER + "," + UserRole.FARMER)]
    [ProducesResponseType(typeof(FeedbackDetail), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateFeedback(CreateFeedback createFeedback)
    {
        try
        {
            var accountId = int.Parse(User.FindFirst("id")?.Value!);
            var result = await _feedbackService.CreateFeedback(createFeedback, accountId);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Landowner, Farmer] Endpoint for update feedback
    /// </summary>
    /// <param name="updateFeedback">An object contains update information</param>
    /// <returns>An feedback updated</returns>
    /// <response code="200">Returns the feedback updated</response>
    /// <response code="400">Returns if input feedback information empty</response>
    /// <response code="401">Returns if invalid authorize.</response>
    [HttpPut]
    [Authorize(Roles = UserRole.LANDOWNER + "," + UserRole.FARMER)]
    [ProducesResponseType(typeof(FeedbackDetail), StatusCodes.Status201Created)]
    public async Task<IActionResult> UpdateFeedback(UpdateFeedback updateFeedback)
    {
        try
        {
            var accountId = int.Parse(User.FindFirst("id")?.Value!);
            var result = await _feedbackService.UpdateFeedback(updateFeedback, accountId);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Landowner, Farmer] Endpoint for delete feedback.
    /// </summary>
    /// <param name="feedbackId">Id of feedback</param>
    /// <returns>Async function</returns>
    /// <response code="204">Returns the feedback deleted</response>
    /// <response code="404">Returns if feedback is not existed.</response>
    /// <response code="401">Returns if invalid authorize.</response>
    [HttpDelete("{feedbackId}")]
    [Authorize(Roles = UserRole.LANDOWNER + "," + UserRole.FARMER)]
    [ProducesResponseType(typeof(FeedbackDetail), StatusCodes.Status201Created)]
    public async Task<IActionResult> DeleteFeedback(int feedbackId)
    {
        try
        {
            var result = await _feedbackService.DeleteFeedback(feedbackId);

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}