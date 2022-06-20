namespace MYRAY.Business.DTOs.Feedback;

public class FeedbackDetail
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public byte NumStar { get; set; }
    public int JobPostId { get; set; }
    public int BelongedId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int CreatedBy { get; set; }
}