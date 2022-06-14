using System.ComponentModel.DataAnnotations;

namespace MYRAY.Business.DTOs.Guidepost;

public class CreateGuidepost
{
    [Required]
    public string? Title { get; set; }
    [Required]
    public string? Content { get; set; }
}