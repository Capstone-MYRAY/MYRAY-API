using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYRAY.Api.Constants;
using MYRAY.Business.Constants;
using MYRAY.Business.Services.Statistic;

namespace MYRAY.Api.Controllers;

/// <summary>
/// Handle request related to Statistic.
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes(MediaType.ApplicationJson)]
[Produces(MediaType.ApplicationJson)]
public class StatisticController : ControllerBase
{
    private readonly IStatisticService _statisticService;

    /// <summary>
    /// Initializes a new instance of the <see cref="StatisticController"/> class.
    /// </summary>
    /// <param name="statisticService"></param>
    public StatisticController(IStatisticService statisticService)
    {
        _statisticService = statisticService;
    }
    
    
    /// <summary>
    /// [Admin, Moderator] Endpoint to get statistic total money
    /// </summary>
    /// <returns>double</returns>
    [HttpGet("money")]
    [Authorize(Roles = UserRole.ADMIN + "," + UserRole.MODERATOR)]
    public async Task<IActionResult> TotalMoney()
    {
        int? moderatorId = null;
        try
        {
            string role = User.Claims.First().Value;
            if (role.Equals("Moderator"))
            {
                moderatorId = int.Parse(User.FindFirst("id")?.Value!);
            }
            var result = await _statisticService.TotalMoney(moderatorId);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// [Admin, Moderator] Endpoint to get statistic total job post
    /// </summary>
    /// <returns>int</returns>
    [HttpGet("jobPost")]
    [Authorize(Roles = UserRole.ADMIN + "," + UserRole.MODERATOR)]
    public async Task<IActionResult> TotalJobPost()
    {
        int? moderatorId = null;
        try
        {
            string role = User.Claims.First().Value;
            if (role.Equals("Moderator"))
            {
                moderatorId = int.Parse(User.FindFirst("id")?.Value!);
            }
            var result = await _statisticService.TotalJobPost(moderatorId);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// [Admin, Moderator] Endpoint to get statistic total Landowner
    /// </summary>
    /// <returns>int</returns>
    [HttpGet("landowner")]
    [Authorize(Roles = UserRole.ADMIN + "," + UserRole.MODERATOR)]
    public async Task<IActionResult> TotalLandowner()
    {
        int? moderatorId = null;
        try
        {
            string role = User.Claims.First().Value;
            if (role.Equals("Moderator"))
            {
                moderatorId = int.Parse(User.FindFirst("id")?.Value!);
            }
            int result = await _statisticService.TotalLandowner(moderatorId);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// [Admin, Moderator] Endpoint to get statistic total Farmer
    /// </summary>
    /// <returns>int</returns>
    [HttpGet("farmer")]
    [Authorize(Roles = UserRole.ADMIN + "," + UserRole.MODERATOR)]
    public async Task<IActionResult> TotalFarmer()
    {
        int? moderatorId = null;
        try
        { 
            string role = User.Claims.First().Value;
            if (role.Equals("Moderator"))
            {
                moderatorId = int.Parse(User.FindFirst("id")?.Value!);
            }
            int result = await _statisticService.TotalFarmer(moderatorId);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
    
    
    
    
    
    
}