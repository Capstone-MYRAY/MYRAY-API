using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYRAY.Api.Constants;
using MYRAY.Business.Constants;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Garden;
using MYRAY.Business.Enums;
using MYRAY.Business.Services.Garden;

namespace MYRAY.Api.Controllers;
/// <summary>
/// Handle request related to garden.
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes(MediaType.ApplicationJson)]
[Produces(MediaType.ApplicationJson)]
public class GardenController : ControllerBase
{
    private readonly IGardenService _gardenService;

    public GardenController(IGardenService gardenService)
    {
        _gardenService = gardenService;
    }
    
    /// <summary>
    /// [Authenticated user] Endpoint for get all garden with condition
    /// </summary>
    /// <param name="searchGarden">An object contains filter criteria.</param>
    /// <param name="sortingDto">An object contains sorting criteria.</param>
    /// <param name="pagingDto">An object contains paging criteria.</param>
    /// <returns>List of garden</returns>
    /// <response code="200">Returns the list of garden.</response>
    /// <response code="204">Returns if list of garden is empty.</response>
    /// <response code="403">Returns if token is access denied.</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDto.CollectiveResponse<GardenDetail>),StatusCodes.Status200OK)]
    public Task<IActionResult> Getgarden(
        [FromQuery] SearchGarden searchGarden,
        [FromQuery] SortingDto<GardenEnum.GardernSortCriteria> sortingDto,
        [FromQuery] PagingDto pagingDto)
    {
        var result =  _gardenService.GetGarden(searchGarden, pagingDto, sortingDto);
        if (result == null)
        {
            return Task.FromResult<IActionResult>(NoContent());
        }

        return Task.FromResult<IActionResult>(Ok(result));
    }

    /// <summary>
    /// [Authenticated user] Endpoint for get garden information by Identifier.
    /// </summary>
    /// <param name="gardenId">An id of garden</param>
    /// <returns>An account</returns>
    /// <response code="200">Returns the garden.</response>
    /// <response code="400">Returns if garden is not existed.</response>
    /// <response code="401">Returns if not authorize</response>
    [HttpGet("{gardenId}")]
    [Authorize]
    [ProducesResponseType(typeof(GardenDetail),StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGardenById(int gardenId)
    {
        try
        {
            var result = await _gardenService.GetGardenById(gardenId);
            
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Landowner] Endpoint for create garden
    /// </summary>
    /// <param name="createGarden">Object contains garden dto</param>
    /// <returns>IActionResult</returns>
    /// <exception cref="Exception">garden is empty data</exception>
    /// <exception cref="Exception">Error if input is empty</exception>
    /// <response code="201">Returns the created garden</response>
    /// <response code="400">Returns if garden input is empty or create error</response>
    /// <response code="401">Returns if invalid authorize</response>
    [HttpPost]
    [Authorize(Roles = UserRole.LANDOWNER)]
    [ProducesResponseType(typeof(GardenDetail),StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateGarden(CreateGarden createGarden)
    {
        try
        {
            var accountId = int.Parse(User.FindFirst("id")?.Value!);
            createGarden.AccountId = accountId;
            var result = await _gardenService.CreateGarden(createGarden);

            return Created(String.Empty, result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Landowner] Endpoint for update garden
    /// </summary>
    /// <param name="updateGarden">An object contains update information</param>
    /// <returns>An garden updated</returns>
    /// <response code="200">Returns the garden updated</response>
    /// <response code="400">Returns if input garden information empty</response>
    /// <response code="401">Returns if invalid authorize.</response>
    [HttpPut]
    [Authorize(Roles = UserRole.LANDOWNER)]
    [ProducesResponseType(typeof(GardenDetail),StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateGarden(UpdateGarden updateGarden)
    {
        try
        {
            var result = await _gardenService.UpdateGarden(updateGarden);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Landowner] Endpoint for delete garden.
    /// </summary>
    /// <param name="gardenId">Id of garden</param>
    /// <returns>Async function</returns>
    /// <response code="204">Returns the garden deleted</response>
    /// <response code="404">Returns if garden is not existed.</response>
    /// <response code="401">Returns if invalid authorize.</response>
    [HttpDelete("{gardenId}")]
    [Authorize(Roles = UserRole.LANDOWNER)]
    [ProducesResponseType(typeof(GardenDetail),StatusCodes.Status201Created)]
    public async Task<IActionResult> DeleteGarden(int gardenId)
    {
        try
        {
            var result = await _gardenService.DeleteGarden(gardenId);

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}