using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYRAY.Api.Constants;
using MYRAY.Business.Constants;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Comment;
using MYRAY.Business.Enums;
using MYRAY.Business.Services.Comment;

namespace MYRAY.Api.Controllers;
/// <summary>
/// Handle request related to comment.
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes(MediaType.ApplicationJson)]
[Produces(MediaType.ApplicationJson)]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    /// <summary>
    /// [Authenticated user] Endpoint for get all comment with condition
    /// </summary>
    /// <param name="searchComment">An object contains filter criteria.</param>
    /// <param name="sortingDto">An object contains sorting criteria.</param>
    /// <param name="pagingDto">An object contains paging criteria.</param>
    /// <param name="guidepostId">Id of Guidepost</param>
    /// <returns>List of comment</returns>
    /// <response code="200">Returns the list of comment.</response>
    /// <response code="204">Returns if list of comment is empty.</response>
    /// <response code="403">Returns if token is access denied.</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDto.CollectiveResponse<CommentDetail>),StatusCodes.Status200OK)]
    public Task<IActionResult> GetComment(
        [FromQuery] SearchComment searchComment,
        [FromQuery] SortingDto<CommentEnum.CommentSortCriteria> sortingDto,
        [FromQuery] PagingDto pagingDto,
        [Required] int guidepostId)
    {
        var result =  _commentService.GetComments(searchComment, pagingDto, sortingDto, guidepostId);
        if (result == null)
        {
            return Task.FromResult<IActionResult>(NoContent());
        }

        return Task.FromResult<IActionResult>(Ok(result));
    }
    
    /// <summary>
    /// [Landowner, Farmer] Endpoint for create comment
    /// </summary>
    /// <param name="createComment">Object contains comment dto</param>
    /// <returns>IActionResult</returns>
    /// <exception cref="Exception">comment is empty data</exception>
    /// <exception cref="Exception">Error if input is empty</exception>
    /// <response code="201">Returns the created comment</response>
    /// <response code="400">Returns if comment input is empty or create error</response>
    /// <response code="401">Returns if invalid authorize</response>
    [HttpPost]
    [Authorize(Roles = UserRole.LANDOWNER + "," + UserRole.FARMER)]
    [ProducesResponseType(typeof(CommentDetail),StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateComment(CreateComment createComment)
    {
        try
        {
            var accountId = int.Parse(User.FindFirst("id")?.Value!);
           
            var result = await _commentService.CreateComment(createComment, accountId);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Landowner, Farmer] Endpoint for update comment
    /// </summary>
    /// <param name="updateComment">An object contains update information</param>
    /// <returns>An comment updated</returns>
    /// <response code="200">Returns the comment updated</response>
    /// <response code="400">Returns if input comment information empty</response>
    /// <response code="401">Returns if invalid authorize.</response>
    [HttpPut]
    [Authorize(Roles = UserRole.LANDOWNER + "," + UserRole.FARMER)]
    [ProducesResponseType(typeof(CommentDetail),StatusCodes.Status201Created)]
    public async Task<IActionResult> UpdateComment(UpdateComment updateComment)
    {
        try
        {
            var accountId = int.Parse(User.FindFirst("id")?.Value!);
            var result = await _commentService.UpdateComment(updateComment, accountId);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Landowner, Farmer] Endpoint for delete comment.
    /// </summary>
    /// <param name="commentId">Id of comment</param>
    /// <returns>Async function</returns>
    /// <response code="204">Returns the comment deleted</response>
    /// <response code="404">Returns if comment is not existed.</response>
    /// <response code="401">Returns if invalid authorize.</response>
    [HttpDelete("{commentId}")]
    [Authorize(Roles = UserRole.LANDOWNER + "," + UserRole.FARMER)]
    [ProducesResponseType(typeof(CommentDetail),StatusCodes.Status201Created)]
    public async Task<IActionResult> DeleteComment(int commentId)
    {
        try
        {
            var result = await _commentService.DeleteComment(commentId);

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}