using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using MYRAY.Business.DTOs.Role;
using MYRAY.Business.Services.Role;
using MediaType = MYRAY.Api.Constants.MediaType;

namespace MYRAY.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes(MediaType.ApplicationJson)]
[Produces(MediaType.ApplicationJson)]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleController"/> class.
    /// </summary>
    /// <param name="roleService">Injection of <see cref="IRoleService"/></param>
    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <summary>
    /// Endpoint for get a list of all role.
    /// </summary>
    /// <returns>List of role.</returns>
    /// <response code="204">Return if list of role is empty.</response>
    /// <response code="200">Return the list of role.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IList<GetRoleDetail>),StatusCodes.Status200OK)]
    public IActionResult GetRoles()
    {
        IList<GetRoleDetail> result = _roleService.GetRoleList().ToList();

        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(result);
    }

    /// <summary>
    /// Endpoint for get detail information of a role.
    /// </summary>
    /// <param name="roleId">ID of Role.</param>
    /// <returns>Detail of a Role.</returns>
    /// <response code="204">Return if role is not exist.</response>
    /// <response code="200">Return role entity.</response>
    [HttpGet("{roleId}")]
    [ProducesResponseType(typeof(GetRoleDetail),StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoleDetail([FromRoute] int? roleId)
    {
        GetRoleDetail result = await _roleService.GetRoleById(roleId);

        if (result == null)
        {
            return NoContent();
        }

        return Ok(result);
    }
}