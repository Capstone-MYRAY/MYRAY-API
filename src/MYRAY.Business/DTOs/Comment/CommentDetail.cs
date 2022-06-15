namespace MYRAY.Business.DTOs.Comment;

public class CommentDetail
{
    public int Id { get; set; }
    public int GuidepostId { get; set; }
    public int CommentBy { get; set; }
    public string? Content { get; set; }
    public DateTime CreateDate { get; set; }
}