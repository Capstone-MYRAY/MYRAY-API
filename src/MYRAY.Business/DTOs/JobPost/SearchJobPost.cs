using System.ComponentModel;

namespace MYRAY.Business.DTOs.JobPost;

public class SearchJobPost
{
    [DefaultValue("")]
    public string Title { get; set; } = "";
    [DefaultValue(null)]
    public int? Status { get; set; } = null;
}