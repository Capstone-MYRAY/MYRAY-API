namespace MYRAY.Business.DTOs.Report;

public class ReportDetail
{
    public int Id { get; set; }
    public int JobPostId { get; set; }
    public string? Content { get; set; }
    public string? ResolveContent { get; set; }
    public int ReportedId { get; set; }
    public string RoleIdReported { get; set; }
    public string ReportedName { get; set; }
    public int CreatedBy { get; set; }
    public string RoleIdCreated { get; set; }
    public string CreatedName { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ResolvedDate { get; set; }
    public int? ResolvedBy { get; set; }
    public int? Status { get; set; }
}