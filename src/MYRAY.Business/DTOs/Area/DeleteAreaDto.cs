using System.ComponentModel.DataAnnotations;

namespace MYRAY.Business.DTOs.Area;

public class DeleteAreaDto
{
    [Required]
    public int Id { get; set; }
}