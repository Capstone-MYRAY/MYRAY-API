namespace MYRAY.Business.DTOs.PostType;

public class CreatePostType
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public double Price { get; set; }
    public string? Color { get; set; }
    public int? Background { get; set; }
}