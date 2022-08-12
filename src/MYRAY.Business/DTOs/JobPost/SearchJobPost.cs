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

    public bool? IsNotEndWork { get; set; } = null;
}