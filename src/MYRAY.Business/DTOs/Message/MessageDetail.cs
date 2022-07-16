namespace MYRAY.Business.DTOs.Message;

public class MessageDetail
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public int FromId { get; set; }
    public int ToId { get; set; }
    public int? JobPostId { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? ConventionId { get; set; }
    public bool? IsRead { get; set; }
}