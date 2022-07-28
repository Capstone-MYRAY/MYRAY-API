namespace MYRAY.Business.DTOs.Guidepost;

public class GuidepostDetail
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int CreatedBy { get; set; }
}