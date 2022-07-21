using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYRAY.Api.Constants;
using MYRAY.Business.Constants;
using MYRAY.Business.DTOs.Attendance;
using MYRAY.Business.Enums;
using MYRAY.Business.Exceptions;
using MYRAY.Business.Services.Attendance;

namespace MYRAY.Api.Controllers;
/// <summary>
/// Handle request related to Account
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes(MediaType.ApplicationJson)]
[Produces(MediaType.ApplicationJson)]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _attendanceService;

    public AttendanceController(IAttendanceService attendanceService)
    {
        _attendanceService = attendanceService;
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
    [ProducesResponseType(typeof(List<AttendanceByJob>),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAttendancesOfDay(
        [Required]int jobPostId,
        [Required]DateTime dateTime, 
        AttendanceEnum.AttendanceStatus? status = null)
    {
        List<AttendanceByJob?> result = await 
            _attendanceService.GetAttendanceByDate(jobPostId, dateTime, status);
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
    [ProducesResponseType(typeof(List<AttendanceDetail>),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAttendances([Required]int jobPostId,[Required]int accountId )
    {
        List<AttendanceDetail> result = await 
            _attendanceService.GetAttendances(jobPostId, accountId);
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
    [ProducesResponseType(typeof(List<AttendanceDetail>),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetDayOffAttendances(int? jobPostId = null)
    {
        var farmerId = int.Parse(User.FindFirst("id")?.Value!);
        List<AttendanceDetail> result = await 
            _attendanceService.GetListDayOffByJob(farmerId, jobPostId);
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
            _attendanceService.GetTotalExpense(jobPostId);
      
        return Ok(result);
    }
    
    /// <summary>
    /// [Landowner] Endpoint for check attendance .
    /// </summary>
    /// <returns>Check Attendance</returns>
    /// <exception cref="Exception">Error if input is empty</exception>
    /// <response code="201">Returns the attendance</response>
    /// <response code="400">Returns if attendance input is empty or create error</response>
    [HttpPost]
    [Authorize(Roles = UserRole.LANDOWNER)]
    [ProducesResponseType(typeof(AttendanceDetail),StatusCodes.Status201Created)]
    public async Task<IActionResult> CheckAttendance([FromBody]CheckAttendance? attendance)
    {
        try
        {
            if (attendance == null)
                throw new Exception("Attendance is empty data");
            var createBy = int.Parse(User.FindFirst("id")?.Value!);
           var result =  await _attendanceService.CreateAttendance(attendance, createBy, attendance.AccountId, attendance.DateAttendance);

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
    /// <returns>Check day off Attendance</returns>
    /// <exception cref="Exception">Error if input is empty</exception>
    /// <response code="201">Returns the attendance</response>
    /// <response code="400">Returns if attendance input is empty or create error</response>
    /// <response code="500">Returns if attendance input is error</response>
    [HttpPost("dayOff")]
    [Authorize(Roles = UserRole.FARMER)]
    [ProducesResponseType(typeof(AttendanceDetail), StatusCodes.Status201Created)]
    public async Task<IActionResult> CheckDayOff([FromBody]RequestDayOff? attendance)
    {
        try
        {
            if (attendance == null)
                throw new Exception("Attendance is empty data");
            var createBy = int.Parse(User.FindFirst("id")?.Value!);
           AttendanceDetail result = await _attendanceService.CreateDayOff(attendance, createBy);

            return Created(String.Empty, result);
        }
        catch (MException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    

}