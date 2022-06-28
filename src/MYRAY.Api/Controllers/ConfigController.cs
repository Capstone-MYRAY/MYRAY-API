using Microsoft.AspNetCore.Mvc;
using MYRAY.Api.Constants;
using MYRAY.Business.DTOs.Config;
using MYRAY.Business.Services.Config;

namespace MYRAY.Api.Controllers;

/// <summary>
/// Handle request related to Area.
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes(MediaType.ApplicationJson)]
[Produces(MediaType.ApplicationJson)]
public class ConfigController : ControllerBase
{
    private readonly IConfigService _configService;

    public ConfigController(IConfigService configService)
    {
        _configService = configService;
    }
    
    /// <summary>
    /// Get Config in system
    /// </summary>
    /// <returns>Object contains config</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ConfigDetail), StatusCodes.Status200OK)]
    public  IActionResult GetConfig()
    {
        var result =  _configService.GetConfig();
        if (result == null)
        {
            return NoContent();
        }

        return Ok(result);
    }
}