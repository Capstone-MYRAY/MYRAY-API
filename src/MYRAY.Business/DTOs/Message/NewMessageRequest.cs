namespace MYRAY.Business.DTOs.Message;

public class NewMessageRequest
{
    public string? Content { get; set; }
    public int FromId { get; set; }
    public int ToId { get; set; }
    public int? JobPostId { get; set; }
    public string? ImageUrl { get; set; }
    
    
}