namespace MYRAY.Business.DTOs.Report;

public class ResolvedReport
{
    public int Id { get; set; }
    public int JobPostId { get; set; }
    public string? ResolveContent { get; set; }
    public DateTime? ResolvedDate { get; set; }
}