using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYRAY.Api.Constants;
using MYRAY.Business.Constants;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.PostType;
using MYRAY.Business.Enums;
using MYRAY.Business.Services.PostType;

namespace MYRAY.Api.Controllers;
/// <summary>
/// Handle request related to post type
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes(MediaType.ApplicationJson)]
[Produces(MediaType.ApplicationJson)]
public class PostTypeController : ControllerBase
{
    private readonly IPostTypeService _postTypeService;

    public PostTypeController(IPostTypeService postTypeService)
    {
        _postTypeService = postTypeService;
    }
    
    /// <summary>
    /// [Authenticated user] Endpoint for get all PostType with condition
    /// </summary>
    /// <param name="searchPostType">An object contains filter criteria.</param>
    /// <param name="sortingDto">An object contains sorting criteria.</param>
    /// <param name="pagingDto">An object contains paging criteria.</param>
    /// <returns>List of PostType</returns>
    /// <response code="200">Returns the list of PostType.</response>
    /// <response code="204">Returns if list of PostType is empty.</response>
    /// <response code="403">Returns if token is access denied.</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDto.CollectiveResponse<PostTypeDetail>),StatusCodes.Status200OK)]
    public Task<IActionResult> GetPostType(
        [FromQuery] SearchPostType searchPostType,
        [FromQuery] SortingDto<PostTypeEnum.PostTypeSortCriteria> sortingDto,
        [FromQuery] PagingDto pagingDto)
    {
        var result =  _postTypeService.GetPostTypes(searchPostType, pagingDto, sortingDto);
        if (result == null)
        {
            return Task.FromResult<IActionResult>(NoContent());
        }

        return Task.FromResult<IActionResult>(Ok(result));
    }

    /// <summary>
    /// [Authenticated user] Endpoint for get PostType information by Identifier.
    /// </summary>
    /// <param name="postTypeId">An id of PostType</param>
    /// <returns>An account</returns>
    /// <response code="200">Returns the PostType.</response>
    /// <response code="400">Returns if PostType is not existed.</response>
    /// <response code="401">Returns if not authorize</response>
    [HttpGet("{postTypeId}")]
    [Authorize]
    [ProducesResponseType(typeof(PostTypeDetail),StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPostTypeById(int postTypeId)
    {
        try
        {
            var result = await _postTypeService.GetPostTypeById(postTypeId);
            
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Admin] Endpoint for create PostType
    /// </summary>
    /// <param name="createPostType">Object contains PostType dto</param>
    /// <returns>IActionResult</returns>
    /// <exception cref="Exception">PostType is empty data</exception>
    /// <exception cref="Exception">Error if input is empty</exception>
    /// <response code="201">Returns the created PostType</response>
    /// <response code="400">Returns if PostType input is empty or create error</response>
    /// <response code="401">Returns if invalid authorize</response>
    [HttpPost]
    [Authorize(Roles = UserRole.ADMIN)]
    [ProducesResponseType(typeof(PostTypeDetail),StatusCodes.Status201Created)]
    public async Task<IActionResult> CreatePostType(CreatePostType createPostType)
    {
        try
        {
          
            var result = await _postTypeService.CreatePostType(createPostType);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Admin] Endpoint for update PostType
    /// </summary>
    /// <param name="updatePostType">An object contains update information</param>
    /// <returns>An PostType updated</returns>
    /// <response code="200">Returns the PostType updated</response>
    /// <response code="400">Returns if input PostType information empty</response>
    /// <response code="401">Returns if invalid authorize.</response>
    [HttpPut]
    [Authorize(Roles = UserRole.ADMIN)]
    [ProducesResponseType(typeof(PostTypeDetail),StatusCodes.Status201Created)]
    public async Task<IActionResult> UpdatePostType(UpdatePostType updatePostType)
    {
        try
        {
            var result = await _postTypeService.UpdatePostType(updatePostType);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Admin] Endpoint for delete PostType.
    /// </summary>
    /// <param name="postTypeId">Id of PostType</param>
    /// <returns>Async function</returns>
    /// <response code="204">Returns the PostType deleted</response>
    /// <response code="404">Returns if PostType is not existed.</response>
    /// <response code="401">Returns if invalid authorize.</response>
    [HttpDelete("{postTypeId}")]
    [Authorize(Roles = UserRole.ADMIN)]
    [ProducesResponseType(typeof(PostTypeDetail),StatusCodes.Status201Created)]
    public async Task<IActionResult> DeletePostType(int postTypeId)
    {
        try
        {
            var result = await _postTypeService.DeletePostType(postTypeId);

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}