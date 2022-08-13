using System.ComponentModel;

namespace MYRAY.Business.DTOs.JobPost;

public class SearchJobPost
{
    public string Title { get; set; } = "";
    public int? Status { get; set; } = null;
    public string Type { get; set; } = "";
    public int? PostTypeId { get; set; } = null;
    public int? StatusWork { get; set; } = null;
    public int? GardenId { get; set; } = null;

    #region Area

    public string? Province { get; set; } = null;

    public string? District { get; set; } = null;

    public string? Commune { get; set; } = null;

    #endregion

    public int? WorkTypeId { get; set; } = null;
    public string? TreeType { get; set; } = null;
    
    public DateTime? StartDateFrom { get; set; } = null;
    public DateTime? StartDateTo { get; set; } = null;

    public float? SalaryFrom { get; set; } = null;
    public float? SalaryTo { get; set; } = null;
    
    public bool? IsNotEndWork { get; set; } = null;
}