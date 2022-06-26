using System.ComponentModel.DataAnnotations;
using MYRAY.Business.Enums;

namespace MYRAY.Business.DTOs.Attendance;

public class CheckAttendance
{
    [Required]
    public int JobPostId { get; set; }

    public int AccountId { get; set; }
    [Required]
    public string? Signature { get; set; }
    public AttendanceEnum.AttendanceStatus Status { get; set; }
}