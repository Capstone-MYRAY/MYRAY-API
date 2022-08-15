using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYRAY.Api.Constants;
using MYRAY.Business.Constants;
using MYRAY.Business.DTOs.Attendance;
using MYRAY.Business.DTOs.SalaryTracking;
using MYRAY.Business.DTOs.SalaryTracking;
using MYRAY.Business.Enums;
using MYRAY.Business.Exceptions;
using MYRAY.Business.Services.SalaryTracking;

namespace MYRAY.Api.Controllers;
/// <summary>
/// Handle request related to Account
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes(MediaType.ApplicationJson)]
[Produces(MediaType.ApplicationJson)]
public class SalaryTrackingController : ControllerBase
{
    private readonly ISalaryTrackingService _salaryTrackingService;

    public SalaryTrackingController(ISalaryTrackingService salaryTrackingService)
    {
        _salaryTrackingService = salaryTrackingService;
    }
    
    /// <summary>
    /// [Authenticated user] Endpoint for get attendance list by date
    /// </summary>
    /// <returns>List of attendance for one date</returns>
    /// <response code="200">Returns the list of attendance of a day.</response>
    /// <response code="204">Return if list of attendance is empty.</response>
    /// <response code="403">Returns if token is access denied.</response>
    [HttpGet("day")]
    [Authorize]
    [ProducesResponseType(typeof(List<SalaryTrackingByJob>),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetSalaryTrackingsOfDay(
        [Required]int jobPostId,
        [Required]DateTime dateTime, 
        SalaryTrackingEnum.SalaryTrackingStatus? status = null)
    {
        List<SalaryTrackingByJob?> result = await 
            _salaryTrackingService.GetSalaryTrackingByDate(jobPostId, dateTime, status);
        if (result == null)
        {
            return NoContent();
        }

        return Ok(result);
    }
    
    /// <summary>
    /// [Authenticated user] Endpoint for get all attendance list
    /// </summary>
    /// <returns>List of attendance</returns>
    /// <response code="200">Returns the list of attendance.</response>
    /// <response code="204">Return if list of attendance is empty.</response>
    /// <response code="403">Returns if token is access denied.</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(List<SalaryTrackingDetail>),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetSalaryTrackings([Required]int jobPostId,[Required]int accountId )
    {
        List<SalaryTrackingDetail> result = await 
            _salaryTrackingService.GetSalaryTrackings(jobPostId, accountId);
        if (result == null)
        {
            return NoContent();
        }

        return Ok(result);
    }
    
    /// <summary>
    /// [Farmer] Endpoint for get all attendance day off list
    /// </summary>
    /// <returns>List of attendance</returns>
    /// <response code="200">Returns the list of day-off attendance.</response>
    /// <response code="204">Return if list of attendance is empty.</response>
    /// <response code="403">Returns if token is access denied.</response>
    [HttpGet("dayOff")]
    [Authorize(Roles = UserRole.FARMER)]
    [ProducesResponseType(typeof(List<SalaryTrackingDetail>),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetDayOffSalaryTrackings(int? jobPostId = null)
    {
        var farmerId = int.Parse(User.FindFirst("id")?.Value!);
        List<SalaryTrackingDetail> result = await 
            _salaryTrackingService.GetListDayOffByJob(farmerId, jobPostId);
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(result);
    }
    
    /// <summary>
    /// [Landowner] Endpoint for get total expense
    /// </summary>
    /// <returns>Total expense to now</returns>
    /// <response code="200">Returns the total expense or zero to no total.</response>
    /// <response code="500">Return if error to get total.</response>
    /// <response code="403">Returns if token is access denied.</response>
    [HttpGet("totalExpense/{jobPostId}")]
    [Authorize(Roles = UserRole.LANDOWNER)]
    [ProducesResponseType(typeof(double?),StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTotalExpense([Required]int jobPostId)
    {
        double? result = await 
            _salaryTrackingService.GetTotalExpense(jobPostId);
      
        return Ok(result);
    }
    
    /// <summary>
    /// [Landowner] Endpoint for check attendance .
    /// </summary>
    /// <returns>Check SalaryTracking</returns>
    /// <exception cref="Exception">Error if input is empty</exception>
    /// <response code="201">Returns the attendance</response>
    /// <response code="400">Returns if attendance input is empty or create error</response>
    [HttpPost]
    [Authorize(Roles = UserRole.LANDOWNER)]
    [ProducesResponseType(typeof(SalaryTrackingDetail),StatusCodes.Status201Created)]
    public async Task<IActionResult> CheckSalaryTracking([FromBody]CheckAttendance? attendance)
    {
        try
        {
            if (attendance == null)
                throw new Exception("SalaryTracking is empty data");
            var createBy = int.Parse(User.FindFirst("id")?.Value!);
           var result =  await _salaryTrackingService.CreateSalaryTracking(attendance, createBy);

            return Created(String.Empty, result);
        }
        catch (MException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// [Farmer] Endpoint for request day off attendance .
    /// </summary>
    /// <returns>Check day off SalaryTracking</returns>
    /// <exception cref="Exception">Error if input is empty</exception>
    /// <response code="201">Returns the attendance</response>
    /// <response code="400">Returns if attendance input is empty or create error</response>
    /// <response code="500">Returns if attendance input is error</response>
    [HttpPost("dayOff")]
    [Authorize(Roles = UserRole.FARMER)]
    [ProducesResponseType(typeof(SalaryTrackingDetail), StatusCodes.Status201Created)]
    public async Task<IActionResult> CheckDayOff([FromBody]RequestDayOff? attendance)
    {
        try
        {
            if (attendance == null)
                throw new Exception("SalaryTracking is empty data");
            var createBy = int.Parse(User.FindFirst("id")?.Value!);
           SalaryTrackingDetail result = await _salaryTrackingService.CreateDayOff(attendance, createBy);

            return Created(String.Empty, result);
        }
        catch (MException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    

}