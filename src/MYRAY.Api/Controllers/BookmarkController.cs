using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYRAY.Api.Constants;
using MYRAY.Business.Constants;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Bookmark;
using MYRAY.Business.Enums;
using MYRAY.Business.Exceptions;
using MYRAY.Business.Services.Bookmark;

namespace MYRAY.Api.Controllers;
/// <summary>
/// Handle request related to bookmark.
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes(MediaType.ApplicationJson)]
[Produces(MediaType.ApplicationJson)]
public class BookmarkController : ControllerBase
{
    private readonly IBookmarkService _bookmarkService;

    public BookmarkController(IBookmarkService bookmarkService)
    {
        _bookmarkService = bookmarkService;
    }

    /// <summary>
    /// [Authenticated user] Endpoint for get all bookmark with condition
    /// </summary>
    /// <param name="sortingDto">An object contains sorting criteria.</param>
    /// <param name="pagingDto">An object contains paging criteria.</param>
    /// <param name="accountId">Account to view bookmark</param>
    /// <returns>List of bookmark</returns>
    /// <response code="200">Returns the list of bookmark.</response>
    /// <response code="204">Return if list of bookmark is empty.</response>
    /// <response code="403">Returns if token is access denied.</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDto.CollectiveResponse<BookmarkDetail>),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> GetBookmarks(
           [FromQuery]SortingDto<BookmarkEnum.BookmarkSortCriterial> sortingDto,
        [FromQuery]PagingDto pagingDto,
           [FromQuery] int accountId)
    {
        ResponseDto.CollectiveResponse<BookmarkDetail> result =
            _bookmarkService.GetBookmarks(pagingDto, sortingDto, accountId);
        if (result == null)
        {
            return Task.FromResult<IActionResult>(NoContent());
        }

        return Task.FromResult<IActionResult>(Ok(result));
    }
       
    /// <summary>
    /// [Landowner, Farmer] Endpoint for create bookmark.
    /// </summary>
    /// <param name="bookmarkId">An object contain information bookmark to Add.</param>
    /// <returns>An bookmark created</returns>
    /// <exception cref="Exception">Error if input is empty</exception>
    /// <response code="201">Returns the bookmark</response>
    /// <response code="500">Returns if bookmark has been marked</response>
    [HttpPost]
    [Authorize(Roles = UserRole.LANDOWNER + "," + UserRole.FARMER)]
    [ProducesResponseType(typeof(BookmarkDetail), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateBookmarkAsync([FromQuery]int bookmarkId)
    {
        try
        {
            var accountId = int.Parse(User.FindFirst("id")?.Value!);
            BookmarkDetail result = await _bookmarkService.CreateBookmarkAsync(accountId, bookmarkId);

            return Created(string.Empty,result);
        }
        catch (MException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Landowner, Farmer] Endpoint for delete bookmark.
    /// </summary>
    /// <param name="bookmarkId">Id of bookmark</param>
    /// <response code="204">Returns the bookmark deleted</response>
    /// <response code="404">Returns if bookmark is not existed.</response>
    [HttpDelete("{bookmarkId}")]
    [Authorize(Roles = UserRole.LANDOWNER + "," + UserRole.FARMER)]
    public async Task<IActionResult> DeleteBookmarkAsync(int bookmarkId)
    {
        try
        {
            var accountId = int.Parse(User.FindFirst("id")?.Value!);
            await _bookmarkService.DeleteBookmarkAsync(accountId, bookmarkId);

            return NoContent();
        }
        catch (Exception e)
        {
            return NotFound(e);
        }
    }
}