using System.ComponentModel.DataAnnotations;

namespace MYRAY.Business.DTOs.TreeType;

public class UpdateTreeType
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string? Type { get; set; }
    
    public string? Description { get; set; }
    public int? Status { get; set; }
}