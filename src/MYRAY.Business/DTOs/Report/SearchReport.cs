namespace MYRAY.Business.DTOs.Report;

public class SearchReport
{
    public int? JobPostId { get; set; } = null;
    public string? Content { get; set; } = "";
    public string? ResolveContent { get; set; } = "";
    public int? ReportedId { get; set; } = null;
    public int? CreatedBy { get; set; } = null;
    public int? ResolvedBy { get; set; } = null;
    public int? Status { get; set; } = null;
}