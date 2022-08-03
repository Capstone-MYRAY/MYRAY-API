using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYRAY.Api.Constants;
using MYRAY.Business.Constants;
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
    private readonly IConfiguration _configuration;

    public ConfigController(IConfigService configService, IConfiguration configuration)
    {
        _configService = configService;
        _configuration = configuration;
    }

    /// <summary>
    /// [Authenticated user] Get Config in system
    /// </summary>
    /// <returns>Object contains config</returns>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(ConfigDetail), StatusCodes.Status200OK)]
    public IActionResult GetConfig()
    {
        var result = _configService.GetConfig();
        if (result == null)
        {
            return NoContent();
        }

        return Ok(result);
    }

    /// <summary>
    /// [Admin] Endpoint for update config
    /// </summary>
    /// <param name="key">Key in store</param>
    /// <param name="value">Value</param>
    [HttpPatch]
    [Authorize(Roles = UserRole.ADMIN)]
    public async Task<IActionResult> UpdateConfig(string key, string value)
    {
        await _configService.SetConfig(key, value);
        return NoContent();
    }
}