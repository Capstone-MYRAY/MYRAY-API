using System.ComponentModel.DataAnnotations;

namespace MYRAY.Business.DTOs.TreeJob;

public class CreateTreeJob
{
    [Required]public int TreeTypeId { get; set; }
    [Required]public int JobPostId { get; set; }
}