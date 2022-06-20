namespace MYRAY.Business.DTOs.Feedback;

public class UpdateFeedback
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public byte NumStar { get; set; }
    public int JobPostId { get; set; }
    public int BelongedId { get; set; }
}