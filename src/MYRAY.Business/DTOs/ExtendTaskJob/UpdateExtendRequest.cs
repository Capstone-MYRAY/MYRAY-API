namespace MYRAY.Business.DTOs.ExtendTaskJob;

public class UpdateExtendRequest
{
    public int Id { get; set; }
    public int? JobPostId { get; set; }
    // public int? RequestBy { get; set; }
    // public int? ApprovedBy { get; set; }
    // public DateTime? CreatedDate { get; set; }
    // public DateTime? ApprovedDate { get; set; }
    public DateTime? ExtendEndDate { get; set; }
    public string? Reason { get; set; }
    // public int? Status { get; set; }
}