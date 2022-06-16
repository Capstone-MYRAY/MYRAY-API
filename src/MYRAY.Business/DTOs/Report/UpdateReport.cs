namespace MYRAY.Business.DTOs.Report;

public class UpdateReport
{
    public int Id { get; set; }
    public int JobPostId { get; set; }
    public string? Content { get; set; }
    public int ReportedId { get; set; }
}