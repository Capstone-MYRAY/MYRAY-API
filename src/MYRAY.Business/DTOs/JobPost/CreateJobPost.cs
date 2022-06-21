using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.DTOs.JobPost;

public class CreateJobPost
{
    [Required]
    public int GardenId { get; set; }
    [Required]
    public int TreeTypeId { get; set; }
    [Required]
    public string Title { get; set; }
    public DateTime? StartJobDate { get; set; }
    public DateTime? EndJobDate { get; set; }
    public int? NumPublishDay { get; set; }
    public string? Description { get; set; }
    public DateTime? PublishedDate { get; set; }

    public PayPerHour? PayPerHourJob { get; set; } = null!;
    public PayPerTask? PayPerTaskJob { get; set; } = null!;

    [DefaultValue(0)]
    public int? UsePoint { get; set; } = null!;
    public int? PostTypeId { get; set; } = null!;
    public DateTime? PinDate { get; set; } = null!;
    public int? NumberPinDay { get; set; } = null!;
}