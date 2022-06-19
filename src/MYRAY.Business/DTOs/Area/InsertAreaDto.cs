using System.ComponentModel.DataAnnotations;

namespace MYRAY.Business.DTOs.Area;

public class InsertAreaDto
{
    [Required]
    public string? Province { get; set; }
    
    [Required]
    public string? District { get; set; }
    
    [Required]
    public string? Commune { get; set; }

    public int? ModeratorId { get; set; }
}