using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MYRAY.Business.DTOs.TreeJob;

namespace MYRAY.Business.DTOs.JobPost;

public class UpdateJobPost
{
    public int Id { get; set; }
    [Required]
    public int GardenId { get; set; }
    public ICollection<CreateTreeJob> TreeJobs { get; set; }
    [Required]
    public string Title { get; set; }
    public DateTime? StartJobDate { get; set; }
    public DateTime? EndJobDate { get; set; }
    public int? NumPublishDay { get; set; }
    public int PublishedBy { get; set; }
    public DateTime? PublishedDate { get; set; }
    public string? Description { get; set; }
    public string? ReasonReject { get; set; }

    public PayPerHour? PayPerHourJob { get; set; } = null!;
    public PayPerTask? PayPerTaskJob { get; set; } = null!;

    [DefaultValue(0)] public int? UsePoint { get; set; } = 0;
    public int? PostTypeId { get; set; } = null!;
    public DateTime? PinDate { get; set; } = null!;
    public int? NumberPinDay { get; set; } = null!;
}