using System.ComponentModel;

namespace MYRAY.Business.DTOs.PostType;

public class SearchPostType
{
    [DefaultValue("")] public string Name { get; set; } = "";
    [DefaultValue("")] public string? Description { get; set; } = "";

    //[DefaultValue("")] public double? Price { get; set; } = null!;
}