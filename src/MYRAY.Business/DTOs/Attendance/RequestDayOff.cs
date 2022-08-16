using System.ComponentModel.DataAnnotations;

namespace MYRAY.Business.DTOs.Attendance;

public class RequestDayOff
{
    [Required]
    public int JobPostId { get; set; }
    
    [Required]
    public DateTime DayOff { get; set; }
    
    [Required]
    public string? Reason { get; set; }
}