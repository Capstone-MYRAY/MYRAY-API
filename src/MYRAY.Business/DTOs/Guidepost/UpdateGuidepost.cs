using System.ComponentModel.DataAnnotations;

namespace MYRAY.Business.DTOs.Guidepost;

public class UpdateGuidepost
{
    [Required]public int Id { get; set; }
    [Required]public string? Title { get; set; }
    [Required]public string? Content { get; set; }
}