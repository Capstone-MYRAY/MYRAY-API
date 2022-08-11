using System.ComponentModel;

namespace MYRAY.Business.DTOs.WorkType;

public class SearchWorkType
{
    [DefaultValue("")] public string Name { get; set; } = ""; 
}