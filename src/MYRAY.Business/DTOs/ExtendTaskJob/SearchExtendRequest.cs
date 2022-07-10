using System.ComponentModel;

namespace MYRAY.Business.DTOs.ExtendTaskJob;

public class SearchExtendRequest
{
    [DefaultValue(null)]public int? RequestBy { get; set; } = null;
    [DefaultValue(null)]public int? ApprovedBy { get; set; } = null;
    [DefaultValue(null)]public int? Status { get; set; } = null;
}