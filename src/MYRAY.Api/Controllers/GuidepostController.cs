using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYRAY.Api.Constants;
using MYRAY.Business.Constants;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Guidepost;
using MYRAY.Business.Enums;
using MYRAY.Business.Services.Guidepost;

namespace MYRAY.Api.Controllers;
/// <summary>
/// Handle request related to guidepost
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes(MediaType.ApplicationJson)]
[Produces(MediaType.ApplicationJson)]
public class GuidepostController :ControllerBase
{
    private readonly IGuidepostService _guidepostService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GuidepostController"/> class.
    /// </summary>
    /// <param name="guidepostService">Injection of <see cref="IGuidepostService"/></param>
    public GuidepostController(IGuidepostService guidepostService)
    {
        _guidepostService = guidepostService;
    }
    
    /// <summary>
    /// [Authenticated user] Endpoint for get all guidepost with condition
    /// </summary>
    /// <param name="searchGuidepost">An object contains filter criteria.</param>
    /// <param name="sortingDto">An object contains sorting criteria.</param>
    /// <param name="pagingDto">An object contains paging criteria.</param>
    /// <returns>List of guidepost</returns>
    /// <response code="200">Returns the list of guidepost.</response>
    /// <response code="204">Returns if list of guidepost is empty.</response>
    /// <response code="403">Returns if token is access denied.</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDto.CollectiveResponse<GuidepostDetail>),StatusCodes.Status200OK)]
    public Task<IActionResult> GetGuidepost(
        [FromQuery] SearchGuidepost searchGuidepost,
        [FromQuery] SortingDto<GuidepostEnum.GuidepostSortCriteria> sortingDto,
        [FromQuery] PagingDto pagingDto)
    {
        var result =  _guidepostService.GetGuideposts(searchGuidepost, pagingDto, sortingDto);
        if (result == null)
        {
            return Task.FromResult<IActionResult>(NoContent());
        }

        return Task.FromResult<IActionResult>(Ok(result));
    }

    /// <summary>
    /// [Authenticated user] Endpoint for get guidepost information by Identifier.
    /// </summary>
    /// <param name="guidepostId">An id of guidepost</param>
    /// <returns>An account</returns>
    /// <response code="200">Returns the guidepost.</response>
    /// <response code="400">Returns if guidepost is not existed.</response>
    /// <response code="401">Returns if not authorize</response>
    [HttpGet("{guidepostId}")]
    [Authorize]
    [ProducesResponseType(typeof(GuidepostDetail),StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGuidepostById(int guidepostId)
    {
        try
        {
            var result = await _guidepostService.GetGuidepostById(guidepostId);
            
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Admin] Endpoint for create guidepost
    /// </summary>
    /// <param name="createGuidepost">Object contains guidepost dto</param>
    /// <returns>IActionResult</returns>
    /// <exception cref="Exception">Guidepost is empty data</exception>
    /// <exception cref="Exception">Error if input is empty</exception>
    /// <response code="201">Returns the created guidepost</response>
    /// <response code="400">Returns if guidepost input is empty or create error</response>
    /// <response code="401">Returns if invalid authorize</response>
    [HttpPost]
    [Authorize(Roles = UserRole.ADMIN)]
    [ProducesResponseType(typeof(GuidepostDetail),StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateGuidepost(CreateGuidepost createGuidepost)
    {
        try
        {
            var accountId = int.Parse(User.FindFirst("id")?.Value!);
            var result = await _guidepostService.CreateGuidepost(createGuidepost, accountId);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Admin] Endpoint for update guidepost
    /// </summary>
    /// <param name="updateGuidepost">An object contains update information</param>
    /// <returns>An guidepost updated</returns>
    /// <response code="200">Returns the guidepost updated</response>
    /// <response code="400">Returns if input guidepost information empty</response>
    /// <response code="401">Returns if invalid authorize.</response>
    [HttpPut]
    [Authorize(Roles = UserRole.ADMIN)]
    [ProducesResponseType(typeof(GuidepostDetail),StatusCodes.Status201Created)]
    public async Task<IActionResult> UpdateGuidepost(UpdateGuidepost updateGuidepost)
    {
        try
        {
            var result = await _guidepostService.UpdateGuidepost(updateGuidepost);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Admin] Endpoint for delete guidepost.
    /// </summary>
    /// <param name="guidepostId">Id of guidepost</param>
    /// <returns>Async function</returns>
    /// <response code="204">Returns the guidepost deleted</response>
    /// <response code="404">Returns if guidepost is not existed.</response>
    /// <response code="401">Returns if invalid authorize.</response>
    [HttpDelete("{guidepostId}")]
    [Authorize(Roles = UserRole.ADMIN)]
    [ProducesResponseType(typeof(GuidepostDetail),StatusCodes.Status201Created)]
    public async Task<IActionResult> DeleteGuidepost(int guidepostId)
    {
        try
        {
            var result = await _guidepostService.DeleteGuidepost(guidepostId);

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}