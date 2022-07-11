namespace MYRAY.Business.DTOs.Attendance;

public class AttendanceByJob
{
    public int JobPostId { get; set; }
    public virtual ICollection<DataTier.Entities.Attendance> Attendances { get; set; }
}