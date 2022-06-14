namespace MYRAY.Business.DTOs.Guidepost;

public class GuidepostDetail
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public int CreateBy { get; set; }
}