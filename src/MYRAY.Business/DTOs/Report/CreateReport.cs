namespace MYRAY.Business.DTOs.Report;

public class CreateReport
{
    public int JobPostId { get; set; }
    public string? Content { get; set; }
    public int ReportedId { get; set; }
}