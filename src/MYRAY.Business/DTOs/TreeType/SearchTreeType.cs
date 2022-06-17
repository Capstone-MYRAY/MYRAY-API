using System.ComponentModel;

namespace MYRAY.Business.DTOs.TreeType;

public class SearchTreeType
{
    [DefaultValue("")]
    public string Type { get; set; } = "";
    
}