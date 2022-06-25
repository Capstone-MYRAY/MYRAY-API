using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYRAY.Api.Constants;
using MYRAY.Business.Constants;
using MYRAY.Business.DTOs.TreeJob;
using MYRAY.Business.Services.TreeJob;

namespace MYRAY.Api;
/// <summary>
/// Handle request related to tree job
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes(MediaType.ApplicationJson)]
[Produces(MediaType.ApplicationJson)]
public class TreeJobController: ControllerBase
{
    private readonly ITreeJobService _treeJobService;

    /// <summary>
    /// Initial new instance of <see cref="TreeJobController"/> class.
    /// </summary>
    /// <param name="treeJobService">Injection of <see cref="ITreeJobService"/></param>
    public TreeJobController(ITreeJobService treeJobService)
    {
        _treeJobService = treeJobService;
    }
    
    /// <summary>
    /// [Authenticated user] Endpoint for get all tree job with condition
    /// </summary>
    /// <returns>List of tree post</returns>
    /// <response code="200">Returns the list of tree job.</response>
    /// <response code="204">Returns if list of tree job is empty.</response>
    /// <response code="403">Returns if token is access denied.</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(List<TreeJobDetail>),StatusCodes.Status200OK)]
    public Task<IActionResult> GetTreeJob([FromQuery]int jobPostId)
    {
        var result = _treeJobService.GetTreeJobs(jobPostId);
        if (result == null)
        {
            return Task.FromResult<IActionResult>(NoContent());
        }

        return Task.FromResult<IActionResult>(Ok(result));
    }
    
    /// <summary>
    /// [Landowner] Endpoint for create tree job
    /// </summary>
    /// <param name="createTreeJob">Object contains tree job dto</param>
    /// <returns>IActionResult</returns>
    /// <exception cref="Exception">Tree job is empty data</exception>
    /// <exception cref="Exception">Error if input is empty</exception>
    /// <response code="201">Returns the created tree job</response>
    /// <response code="400">Returns if tree job input is empty or create error</response>
    /// <response code="401">Returns if invalid authorize</response>
    [HttpPost]
    [Authorize(Roles = UserRole.LANDOWNER)]
    [ProducesResponseType(typeof(TreeJobDetail),StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateGuidepost(CreateTreeJob createTreeJob)
    {
        try
        {
            var result = await _treeJobService.CreateTreeJobs(createTreeJob);

            return Created(string.Empty,result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    
    /// <summary>
    /// [Landowner] Endpoint for delete tree job.
    /// </summary>
    /// <param name="deleteTreeJob">Id of tree job</param>
    /// <returns>Async function</returns>
    /// <response code="204">Returns the tree job deleted</response>
    /// <response code="404">Returns if tree job is not existed.</response>
    /// <response code="401">Returns if invalid authorize.</response>
    [HttpDelete]
    [Authorize(Roles = UserRole.LANDOWNER)]
    public async Task<IActionResult> DeleteTreeJob([FromQuery] DeleteTreeJob deleteTreeJob)
    {
        try
        {
            var result = await _treeJobService.DeleteTreeJobs(deleteTreeJob);

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

}