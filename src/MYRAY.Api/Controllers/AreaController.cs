using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYRAY.Api.Constants;
using MYRAY.Business.Constants;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Area;
using MYRAY.Business.Enums;
using MYRAY.Business.Exceptions;
using MYRAY.Business.Services.Area;

namespace MYRAY.Api.Controllers;
/// <summary>
/// Handle request related to Area.
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes(MediaType.ApplicationJson)]
[Produces(MediaType.ApplicationJson)]
public class AreaController : ControllerBase
{
    private readonly IAreaService _areaService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AreaController"/> class.
    /// </summary>
    /// <param name="areaService">Injection of <see cref="IAreaService"/></param>
    public AreaController(IAreaService areaService)
    {
        _areaService = areaService;
    }

    /// <summary>
    /// [Authenticated user] Endpoint for get all area with condition
    /// </summary>
    /// <param name="searchAreaDto">An object contains filter criteria.</param>
    /// <param name="sortingDto">An object contains sorting criteria.</param>
    /// <param name="pagingDto">An object contains paging criteria.</param>
    /// <returns>List of area</returns>
    /// <response code="200">Returns the list of area.</response>
    /// <response code="204">Return if list of area is empty.</response>
    /// <response code="403">Returns if token is access denied.</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDto.CollectiveResponse<GetAreaDetail>),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAreas(
        [FromQuery]SearchAreaDto searchAreaDto, 
        [FromQuery]SortingDto<AreaEnum.AreaSortCriteria> sortingDto,
        [FromQuery]PagingDto pagingDto)
    {
        try
        {
            ResponseDto.CollectiveResponse<GetAreaDetail> result = 
            _areaService.GetAreas(pagingDto, sortingDto, searchAreaDto);
            if (!result.ListObject.Any())
            {
                return NoContent();
            }
            
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Authenticated user] Endpoint for get area information by ID
    /// </summary>
    /// <param name="areaId">An id of area</param>
    /// <returns>An Area.</returns>
    /// <response code="200">Returns the area.</response>
    /// <response code="204">Return if area is not existed.</response>
    /// <response code="400">Return if area id is empty or not number.</response>
    /// <response code="403">Returns if token is access denied.</response>
    [HttpGet("{areaId}")]
    [Authorize]
    [ProducesResponseType(typeof(GetAreaDetail),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorCustomMessage),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAreaByIdAsync(int? areaId)
    {
        if (areaId == null || !int.TryParse(areaId.ToString(), out _))
        {
            return BadRequest(new ErrorCustomMessage()
            {
                Message = "Area ID is null or not number",
                Target = nameof(areaId)
            });
        }
            
        GetAreaDetail result = await _areaService.GetAreaByIdAsync(areaId);
        
        if (result == null)
        {
            return NoContent();
        }
        return Ok(result);
    }

    /// <summary>
    /// [Admin] Endpoint for create area.
    /// </summary>
    /// <param name="areaDto">An object contain information area to Add.</param>
    /// <returns>An area created</returns>
    /// <exception cref="Exception">Error if input is empty</exception>
    /// <response code="201">Returns the area</response>
    /// <response code="400">Returns if area input is empty or create error</response>
    [HttpPost]
    [Authorize(Roles = UserRole.ADMIN)]
    [ProducesResponseType(typeof(GetAreaDetail), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAreaAsync([FromBody]InsertAreaDto? areaDto)
    {
        try
        {
            if (areaDto == null)
                throw new Exception("Area is empty data");
            GetAreaDetail result = await _areaService.CreateAreaAsync(areaDto);

            return Created(string.Empty,result);
        }
        catch (MException e)
        {
            return BadRequest(new ErrorCustomMessage {Message = e.Message, Target = nameof(areaDto)});
        }
    }

    /// <summary>
    /// [Admin] Endpoint for admin edit area.
    /// </summary>
    /// <param name="updateAreaDto">An object contains update information area.</param>
    /// <returns>An area updated.</returns>
    /// <response code="200">Returns the area updated</response>
    /// <response code="400">Returns if input area information empty</response>
    /// <response code="404">Returns if area update is not existed.</response>
    [HttpPut]
    [Authorize(Roles = UserRole.ADMIN)]
    [ProducesResponseType(typeof(UpdateAreaDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAreaAsync(
        [FromBody] UpdateAreaDto updateAreaDto)
    {
        try
        {
            UpdateAreaDto updateArea = await _areaService.UpdateAreaAsync(updateAreaDto);

            return Ok(updateArea);
        }
        catch (MException e)
        {
            return BadRequest(e);
        }
        catch (Exception)
        {
            return NotFound(new MException(StatusCodes.Status404NotFound, "ID area is not existed.", nameof(updateAreaDto.Id)));
        }
    }

    /// <summary>
    /// [Admin] Endpoint for delete area.
    /// </summary>
    /// <param name="areaId">Id of area</param>
    /// <response code="204">Returns the area deleted</response>
    /// <response code="404">Returns if area is not existed.</response>
    [HttpDelete("{areaId}")]
    [Authorize(Roles = UserRole.ADMIN)]
    public async Task<IActionResult> DeleteAreaAsync(int? areaId)
    {
        try
        {
            await _areaService.DeleteAreaAsync(areaId);

            return NoContent();
        }
        catch (Exception e)
        {
            return NotFound(e);
        }
    }

}