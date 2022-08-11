using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYRAY.Api.Constants;
using MYRAY.Business.Constants;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.WorkType;
using MYRAY.Business.Enums;
using MYRAY.Business.Services.WorkType;

namespace MYRAY.Api.Controllers;


/// <summary>
/// Handle request related to work type
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes(MediaType.ApplicationJson)]
[Produces(MediaType.ApplicationJson)]
public class WorkTypeController : ControllerBase
{
    private readonly IWorkTypeService _workTypeService;

    /// <summary>
    /// New Instance of <see cref="WorkTypeController"/> class.
    /// </summary>
    /// <param name="workTypeService"></param>
    public WorkTypeController(IWorkTypeService workTypeService)
    {
        _workTypeService = workTypeService;
    }
    
    /// <summary>
    /// [Authenticated user] Endpoint for get all WorkType with condition
    /// </summary>
    /// <param name="searchWorkType">An object contains filter criteria.</param>
    /// <param name="sortingDto">An object contains sorting criteria.</param>
    /// <param name="pagingDto">An object contains paging criteria.</param>
    /// <returns>List of WorkType</returns>
    /// <response code="200">Returns the list of WorkType.</response>
    /// <response code="204">Returns if list of WorkType is empty.</response>
    /// <response code="403">Returns if token is access denied.</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDto.CollectiveResponse<WorkTypeDetail>),StatusCodes.Status200OK)]
    public Task<IActionResult> GetWorkType(
        [FromQuery] SearchWorkType searchWorkType,
        [FromQuery] SortingDto<WorkTypeEnum.WorkTypeSortCriteria> sortingDto,
        [FromQuery] PagingDto pagingDto)
    {
        var result =  _workTypeService.GetWorkTypes(searchWorkType, pagingDto, sortingDto);
        if (result == null)
        {
            return Task.FromResult<IActionResult>(NoContent());
        }

        return Task.FromResult<IActionResult>(Ok(result));
    }

    /// <summary>
    /// [Authenticated user] Endpoint for get WorkType information by Identifier.
    /// </summary>
    /// <param name="workTypeId">An id of WorkType</param>
    /// <returns>An account</returns>
    /// <response code="200">Returns the WorkType.</response>
    /// <response code="400">Returns if WorkType is not existed.</response>
    /// <response code="401">Returns if not authorize</response>
    [HttpGet("{workTypeId}")]
    [Authorize]
    [ProducesResponseType(typeof(WorkTypeDetail),StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWorkTypeById(int workTypeId)
    {
        try
        {
            var result = await _workTypeService.GetWorkTypeById(workTypeId);
            
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Admin] Endpoint for create WorkType
    /// </summary>
    /// <param name="createWorkType">Object contains WorkType dto</param>
    /// <returns>IActionResult</returns>
    /// <exception cref="Exception">WorkType is empty data</exception>
    /// <exception cref="Exception">Error if input is empty</exception>
    /// <response code="201">Returns the created WorkType</response>
    /// <response code="400">Returns if WorkType input is empty or create error</response>
    /// <response code="401">Returns if invalid authorize</response>
    [HttpPost]
    [Authorize(Roles = UserRole.ADMIN)]
    [ProducesResponseType(typeof(WorkTypeDetail),StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateWorkType(CreateWorkType createWorkType)
    {
        try
        {
          
            var result = await _workTypeService.CreateWorkType(createWorkType);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Admin] Endpoint for update WorkType
    /// </summary>
    /// <param name="updateWorkType">An object contains update information</param>
    /// <returns>An WorkType updated</returns>
    /// <response code="200">Returns the WorkType updated</response>
    /// <response code="400">Returns if input WorkType information empty</response>
    /// <response code="401">Returns if invalid authorize.</response>
    [HttpPut]
    [Authorize(Roles = UserRole.ADMIN)]
    [ProducesResponseType(typeof(WorkTypeDetail),StatusCodes.Status201Created)]
    public async Task<IActionResult> UpdateWorkType(UpdateWorkType updateWorkType)
    {
        try
        {
            var result = await _workTypeService.UpdateWorkType(updateWorkType);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Admin] Endpoint for delete WorkType.
    /// </summary>
    /// <param name="workTypeId">Id of WorkType</param>
    /// <returns>Async function</returns>
    /// <response code="204">Returns the WorkType deleted</response>
    /// <response code="404">Returns if WorkType is not existed.</response>
    /// <response code="401">Returns if invalid authorize.</response>
    [HttpDelete("{workTypeId}")]
    [Authorize(Roles = UserRole.ADMIN)]
    [ProducesResponseType(typeof(WorkTypeDetail),StatusCodes.Status201Created)]
    public async Task<IActionResult> DeleteWorkType(int workTypeId)
    {
        try
        {
            var result = await _workTypeService.DeleteWorkType(workTypeId);

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}