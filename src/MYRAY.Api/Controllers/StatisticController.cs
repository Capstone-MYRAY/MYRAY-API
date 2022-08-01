using System.ComponentModel.DataAnnotations;
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
    /// [Admin, Moderator] Endpoint to get statistic total 
    /// </summary>
    [HttpGet]
    [Authorize(Roles = UserRole.ADMIN + "," + UserRole.MODERATOR)]
    public async Task<IActionResult> Statistic()
    {
        int? moderatorId = null;
        try
        {
            string role = User.Claims.First().Value;
            if (role.Equals("Moderator"))
            {
                moderatorId = int.Parse(User.FindFirst("id")?.Value!);
            }
            var result = await _statisticService.GetStatistic(moderatorId);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// [Admin, Moderator] Endpoint to get statistic total money for year
    /// </summary>
    [HttpGet("year")]
    [Authorize(Roles = UserRole.ADMIN + "," + UserRole.MODERATOR)]
    public async Task<IActionResult> StatisticByYear([Required] int year)
    {
        int? moderatorId = null;
        try
        {
         
            var result = await _statisticService.GetStatisticByYear(year ,moderatorId);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
    
}