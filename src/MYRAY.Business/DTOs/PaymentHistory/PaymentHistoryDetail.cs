namespace MYRAY.Business.DTOs.PaymentHistory;

public class PaymentHistoryDetail
{
    public int Id { get; set; }
    public int JobPostId { get; set; }
    public int BelongedId { get; set; }
    public double? ActualPrice { get; set; }
    public double? BalanceFluctuation { get; set; }
    public double? Balance { get; set; }
    public int? UsedPoint { get; set; }
    public int? EarnedPoint { get; set; }
    public string? Message { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? Status { get; set; }
    public double? JobPostPrice { get; set; }
    public int? PointPrice { get; set; }
}