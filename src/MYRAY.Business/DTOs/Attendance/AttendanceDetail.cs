namespace MYRAY.Business.DTOs.Attendance;

public class AttendanceDetail
{
    public int Id { get; set; }
    public DateTime? Date { get; set; }
    public double? Salary { get; set; }
    public int? BonusPoint { get; set; }
    public string? Signature { get; set; }
    public int? Status { get; set; }
    public int AppliedJobId { get; set; }
    public string AppliedJobTitle { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? Reason { get; set; }
    public int AccountId { get; set; }
    public string? ImageUrl { get; set; }
    public string Fullname { get; set; }
    public string PhoneNumber { get; set; }
}