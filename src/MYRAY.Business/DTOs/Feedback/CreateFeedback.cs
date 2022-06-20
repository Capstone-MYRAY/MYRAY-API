namespace MYRAY.Business.DTOs.Feedback;

public class CreateFeedback
{
    public string? Content { get; set; }
    public byte NumStar { get; set; }
    public int JobPostId { get; set; }
    public int BelongedId { get; set; }
}