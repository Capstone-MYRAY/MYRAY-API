using System.ComponentModel.DataAnnotations;

namespace MYRAY.Business.DTOs.Area;

public class UpdateAreaDto : InsertAreaDto
{
    [Required]
    public int Id { get; set; }
    
}