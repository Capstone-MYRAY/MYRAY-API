using System.ComponentModel;

namespace MYRAY.Business.DTOs.Garden;

public class SearchGarden
{
    [DefaultValue(null)]public int? AreaId { get; set; } = null;
    [DefaultValue(null)]public int? AccountId { get; set; } = null;
    [DefaultValue("")]public string Name { get; set; }  = "";
    [DefaultValue("")]public string? Description { get; set; } = "";
}