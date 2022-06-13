using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYRAY.Api.Constants;
using MYRAY.Business.Constants;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.TreeType;
using MYRAY.Business.Enums;
using MYRAY.Business.Services.TreeType;

namespace MYRAY.Api.Controllers;
/// <summary>
/// Handle request related to tree type
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes(MediaType.ApplicationJson)]
[Produces(MediaType.ApplicationJson)]
public class TreeTypeController : ControllerBase
{
    private readonly ITreeTypeService _treeTypeService;

    /// <summary>
    /// Initializes a new instance of the <see cref="TreeTypeController"/> class.
    /// </summary>
    /// <param name="treeTypeService">Injection of <see cref="ITreeTypeService"/></param>
    public TreeTypeController(ITreeTypeService treeTypeService)
    {
        _treeTypeService = treeTypeService;
    }

    /// <summary>
    /// [Authenticated user] Endpoint for get all tree type with condition
    /// </summary>
    /// <param name="searchTreeType">An object contains filter criteria.</param>
    /// <param name="sortingDto">An object contains sorting criteria.</param>
    /// <param name="pagingDto">An object contains paging criteria.</param>
    /// <returns>List of tree type</returns>
    /// <response code="200">Returns the list of tree type.</response>
    /// <response code="204">Returns if list of tree type is empty.</response>
    /// <response code="403">Returns if token is access denied.</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDto.CollectiveResponse<TreeTypeDetail>),StatusCodes.Status200OK)]
    public Task<IActionResult> GetTreeTypes(
        [FromQuery] SearchTreeType searchTreeType,
        [FromQuery] SortingDto<TreeTypeEnum.TreeTypeSortCriteria> sortingDto,
        [FromQuery] PagingDto pagingDto)
    {
        var result =  _treeTypeService.GetTreeTypes(searchTreeType, pagingDto, sortingDto);
        if (result == null)
        {
            return Task.FromResult<IActionResult>(NoContent());
        }

        return Task.FromResult<IActionResult>(Ok(result));
    }

    /// <summary>
    /// [Authenticated user] Endpoint for get tree type information by Identifier.
    /// </summary>
    /// <param name="treeTypeId">An id of tree type</param>
    /// <returns>An account</returns>
    /// <response code="200">Returns the tree type.</response>
    /// <response code="400">Returns if tree type is not existed.</response>
    /// <response code="401">Returns if not authorize</response>
    [HttpGet("{treeTypeId}")]
    [Authorize]
    [ProducesResponseType(typeof(TreeTypeDetail),StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTreeTypeById(int treeTypeId)
    {
        try
        {
            var result = await _treeTypeService.GetTreeTypeById(treeTypeId);
            
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Admin] Endpoint for create tree type
    /// </summary>
    /// <param name="createTreeType">Object contains tree type dto</param>
    /// <returns>IActionResult</returns>
    /// <exception cref="Exception">Tree type is empty data</exception>
    /// <exception cref="Exception">Error if input is empty</exception>
    /// <response code="201">Returns the created tree type</response>
    /// <response code="400">Returns if tree type input is empty or create error</response>
    /// <response code="401">Returns if invalid authorize</response>
    [HttpPost]
    [Authorize(Roles = UserRole.ADMIN)]
    [ProducesResponseType(typeof(TreeTypeDetail),StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateTreeType(CreateTreeType createTreeType)
    {
        try
        {
            var result = await _treeTypeService.CreateTreeType(createTreeType);

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Admin] Endpoint for tree type
    /// </summary>
    /// <param name="updateTreeType">An object contains update information</param>
    /// <returns>An account updated</returns>
    /// <response code="200">Returns the tree type updated</response>
    /// <response code="400">Returns if input tree type information empty</response>
    /// <response code="401">Returns if invalid authorize.</response>
    [HttpPut]
    [Authorize(Roles = UserRole.ADMIN)]
    [ProducesResponseType(typeof(TreeTypeDetail),StatusCodes.Status201Created)]
    public async Task<IActionResult> UpdateTreeType(UpdateTreeType updateTreeType)
    {
        try
        {
            var result = await _treeTypeService.UpdateTreeType(updateTreeType);

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Admin] Endpoint for delete tree type.
    /// </summary>
    /// <param name="treeTypeId">Id of  tree type</param>
    /// <returns>Async function</returns>
    /// <response code="204">Returns the tree type deleted</response>
    /// <response code="404">Returns if tree type is not existed.</response>
    /// <response code="401">Returns if invalid authorize.</response>
    [HttpDelete("{treeTypeId}")]
    [Authorize(Roles = UserRole.ADMIN)]
    [ProducesResponseType(typeof(TreeTypeDetail),StatusCodes.Status201Created)]
    public async Task<IActionResult> DeleteTreeType(int treeTypeId)
    {
        try
        {
            var result = await _treeTypeService.DeleteTreeType(treeTypeId);

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

}