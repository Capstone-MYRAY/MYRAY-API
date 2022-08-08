using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYRAY.Api.Constants;
using MYRAY.Business.Constants;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Report;
using MYRAY.Business.Enums;
using MYRAY.Business.Services.Report;

namespace MYRAY.Api.Controllers;

/// <summary>
/// Handle request related to report
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes(MediaType.ApplicationJson)]
[Produces(MediaType.ApplicationJson)]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    /// <summary>
    /// [Authenticated user] Endpoint for get all report with condition
    /// </summary>
    /// <param name="searchReport">An object contains filter criteria.</param>
    /// <param name="sortingDto">An object contains sorting criteria.</param>
    /// <param name="pagingDto">An object contains paging criteria.</param>
    /// <returns>List of report</returns>
    /// <response code="200">Returns the list of report.</response>
    /// <response code="204">Returns if list of report is empty.</response>
    /// <response code="403">Returns if token is access denied.</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDto.CollectiveResponse<ReportDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReport(
        [FromQuery] SearchReport searchReport,
        [FromQuery] SortingDto<ReportEnum.ReportSortCriterial> sortingDto,
        [FromQuery] PagingDto pagingDto)
    {
        var result = _reportService.GetReport(searchReport, pagingDto, sortingDto);
        if (result == null)
        {
            return NoContent();
        }

        return Ok(result);
    }

    
    /// <summary>
    /// [Authenticated user] Endpoint for get all report with reported id.
    /// </summary>
    /// <param name="reportId">Id of account has been reported.</param>
    /// <returns>List of report.</returns>
    /// <response code="200">Returns the list of report.</response>
    /// <response code="204">Returns if list of report is empty.</response>
    /// <response code="403">Returns if token is access denied.</response>
    [HttpGet("reported/{reportId}")]
    [Authorize]
    [ProducesResponseType(typeof(List<ReportDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReportByReportedId(int reportId)
    {
        var result = await _reportService.GetReportByReportedId(reportId);
        if (result != null && !result.Any())
        {
            return NoContent();
        }

        return Ok(result);
    }

    /// <summary>
    /// [Authenticated user] Endpoint for get report information by Identifier.
    /// </summary>
    /// <param name="reportId">An id of report</param>
    /// <returns>An account</returns>
    /// <response code="200">Returns the report.</response>
    /// <response code="400">Returns if report is not existed.</response>
    /// <response code="401">Returns if not authorize</response>
    [HttpGet("{reportId}")]
    [Authorize]
    [ProducesResponseType(typeof(ReportDetail), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReportById(int reportId)
    {
        try
        {
            var result = await _reportService.GetReportById(reportId);

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Authenticated user] Endpoint for get a report information with condition.
    /// </summary>
    /// <returns>An account</returns>
    /// <response code="200">Returns the report.</response>
    /// <response code="400">Returns if report is not existed.</response>
    /// <response code="401">Returns if not authorize</response>
    [HttpGet("one")]
    [Authorize]
    [ProducesResponseType(typeof(ReportDetail), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOneReportById(
        [Required]int jobPostId, 
        [Required]int reportedId, 
        [Required]int createById)
    {
        try
        {
            var result = await _reportService.GetOneReportById(jobPostId, reportedId, createById);
            if (result == null)
                return NoContent();
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Landowner, Farmer] Endpoint for create report
    /// </summary>
    /// <param name="createReport">Object contains report dto</param>
    /// <returns>IActionResult</returns>
    /// <exception cref="Exception">report is empty data</exception>
    /// <exception cref="Exception">Error if input is empty</exception>
    /// <response code="201">Returns the created report</response>
    /// <response code="400">Returns if report input is empty or create error</response>
    /// <response code="401">Returns if invalid authorize</response>
    [HttpPost]
    [Authorize(Roles = UserRole.LANDOWNER + "," + UserRole.FARMER)]
    [ProducesResponseType(typeof(ReportDetail), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateReport(CreateReport createReport)
    {
        try
        {
            var accountId = int.Parse(User.FindFirst("id")?.Value!);
            var result = await _reportService.CreateReport(createReport, accountId);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Landowner, Farmer] Endpoint for update report
    /// </summary>
    /// <param name="updateReport">An object contains update information</param>
    /// <returns>An report updated</returns>
    /// <response code="200">Returns the report updated</response>
    /// <response code="400">Returns if input report information empty</response>
    /// <response code="401">Returns if invalid authorize.</response>
    [HttpPut]
    [Authorize(Roles = UserRole.LANDOWNER + "," + UserRole.FARMER)]
    [ProducesResponseType(typeof(ReportDetail), StatusCodes.Status201Created)]
    public async Task<IActionResult> UpdateReport(UpdateReport updateReport)
    {
        try
        {
            var accountId = int.Parse(User.FindFirst("id")?.Value!);
            var result = await _reportService.UpdateReport(updateReport, accountId);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// [Admin, Moderator] Endpoint for resolve report
    /// </summary>
    /// <param name="resolvedReport">An object contains resolve information</param>
    /// <returns>An report resolve</returns>
    /// <response code="200">Returns the report resolved</response>
    /// <response code="400">Returns if input report information empty</response>
    /// <response code="401">Returns if invalid authorize.</response>
    [HttpPut("resolve")]
    [Authorize(Roles = UserRole.ADMIN + "," + UserRole.MODERATOR)]
    [ProducesResponseType(typeof(ReportDetail), StatusCodes.Status201Created)]
    public async Task<IActionResult> ResolveReport(ResolvedReport resolvedReport)
    {
        try
        {
            var accountId = int.Parse(User.FindFirst("id")?.Value!);
            var result = await _reportService.ResolvedReport(resolvedReport, accountId);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }


    /// <summary>
    /// [Landowner, Farmer] Endpoint for delete report.
    /// </summary>
    /// <param name="reportId">Id of report</param>
    /// <returns>Async function</returns>
    /// <response code="204">Returns the report deleted</response>
    /// <response code="404">Returns if report is not existed.</response>
    /// <response code="401">Returns if invalid authorize.</response>
    [HttpDelete("{reportId}")]
    [Authorize(Roles = UserRole.LANDOWNER + "," + UserRole.FARMER)]
    [ProducesResponseType(typeof(ReportDetail), StatusCodes.Status201Created)]
    public async Task<IActionResult> DeleteReport(int reportId)
    {
        try
        {
            var result = await _reportService.DeleteReport(reportId);

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}