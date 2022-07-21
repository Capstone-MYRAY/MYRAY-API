using System.ComponentModel;

namespace MYRAY.Business.DTOs.Feedback;

public class SearchFeedback
{
    // public string? Content { get; set; } = "";
    [DefaultValue(null)]public byte? NumStar { get; set; }  = null;
    [DefaultValue(null)]public int? JobPostId { get; set; } = null;
    // public int? BelongedId { get; set; } = null;
    [DefaultValue(null)]public int? CreatedBy { get; set; } = null;
}