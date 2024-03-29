namespace MYRAY.Business.DTOs.PostType;

public class UpdatePostType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public double Price { get; set; }
    public string? Color { get; set; }
    public string? Background { get; set; }
}