using MYRAY.Business.DTOs.Garden;
using MYRAY.Business.DTOs.TreeJob;
using MYRAY.Business.Enums;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.DTOs.JobPost;

public class JobPostDetail
{
    public int Id { get; set; }
    public int GardenId { get; set; }
    public string GardenName { get; set; }
    public string Address { get; set; }
    public ICollection<TreeJobDetail> TreeJobs { get; set; }
    public string TreeName { get; set; }
    public string Title { get; set; } = null!;
    public string Type { get; set; } = null!;
    public DateTime? StartJobDate { get; set; }
    public DateTime? EndJobDate { get; set; }
    public int? NumPublishDay { get; set; }
    public int PublishedBy { get; set; }
    public string PublishedName { get; set; }
    public DateTime? PublishedDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
    public int? StatusWork { get; set; }
    public string? ReasonReject { get; set; }
    public int? PostTypeId { get; set; }
    public string? Color { get; set; }
    public string? Background { get; set; }
    
    public PayPerHour PayPerHourJob { get; set; } = null!;
    public PayPerTask PayPerTaskJob { get; set; } = null!;
    
   
}