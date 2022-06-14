using System.ComponentModel;

namespace MYRAY.Business.DTOs.Guidepost;

public class SearchGuidepost
{
    [DefaultValue("")] public string Title { get; set; } = "";
    [DefaultValue("")] public string? Content { get; set; } = "";
    [DefaultValue(null)] public int? CreateBy { get; set; } = null;
}