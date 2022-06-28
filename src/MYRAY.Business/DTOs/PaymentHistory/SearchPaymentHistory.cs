using System.ComponentModel;

namespace MYRAY.Business.DTOs.PaymentHistory;

public class SearchPaymentHistory
{
    [DefaultValue(null)]public int? JobPostId { get; set; }= null;
    [DefaultValue(null)]public int? CreatedBy { get; set; }= null;
    [DefaultValue(null)] public int? Status { get; set; } = null;
}