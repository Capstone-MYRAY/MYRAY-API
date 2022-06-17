using System.ComponentModel;

namespace MYRAY.Business.DTOs.Area;

public class SearchAreaDto
{
    [DefaultValue("")] public string Province { get; set; } = "";
    [DefaultValue("")] public string District { get; set; } = "";
    [DefaultValue("")] public string Commune { get; set; } = "";
    [DefaultValue(null)] public int? Status { get; set; } = null;
}