namespace MYRAY.Business.DTOs.Report;

public class SearchReport
{
    public int JobPostId { get; set; }
    public string? Content { get; set; } = "";
    public string? ResolveContent { get; set; } = "";
    public int ReportedId { get; set; }
    public int CreatedBy { get; set; }
    public int? ResolvedBy { get; set; }
}