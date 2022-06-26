using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYRAY.Api.Constants;
using MYRAY.Business.Constants;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Area;
using MYRAY.Business.DTOs.Attendance;
using MYRAY.Business.Enums;
using MYRAY.Business.Exceptions;
using MYRAY.Business.Services.Attendance;
using MYRAY.DataTier.Entities;

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
    /// [Authenticated user] Endpoint for get all attendance list
    /// </summary>
    /// <returns>List of area</returns>
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
    /// [Landowner] Endpoint for check attendance .
    /// </summary>
    /// <returns>Check Attendance</returns>
    /// <exception cref="Exception">Error if input is empty</exception>
    /// <response code="201">Returns the attendance</response>
    /// <response code="400">Returns if attendance input is empty or create error</response>
    [HttpPost]
    [Authorize(Roles = UserRole.LANDOWNER)]
    [ProducesResponseType(typeof(GetAreaDetail), StatusCodes.Status201Created)]
    public async Task<IActionResult> CheckAttendance([FromBody]CheckAttendance? attendance)
    {
        try
        {
            if (attendance == null)
                throw new Exception("Attendance is empty data");
            var createBy = int.Parse(User.FindFirst("id")?.Value!);
            await _attendanceService.CreateAttendance(attendance, createBy, attendance.AccountId);

            return Created(String.Empty, null);
        }
        catch (MException e)
        {
            return BadRequest(e.Message);
        }
    }

}