using System.ComponentModel.DataAnnotations;

namespace MYRAY.Business.DTOs.TreeType;

public class CreateTreeType
{
    [Required(ErrorMessage = "Type name is required")]
    public string Type { get; set; } = null!;
    public string? Description { get; set; }
}