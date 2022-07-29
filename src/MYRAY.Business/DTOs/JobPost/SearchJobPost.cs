using System.ComponentModel;

namespace MYRAY.Business.DTOs.JobPost;

public class SearchJobPost
{
    [DefaultValue("")]
    public string Title { get; set; } = "";
    [DefaultValue(null)]
    public int? Status { get; set; } = null;
    public string Type { get; set; } = "";
    public int? PostTypeId { get; set; } = null;
    [DefaultValue(null)] public int? StatusWork { get; set; } = null;
}