namespace MYRAY.Business.Enums;

public class PaymentHistoryEnum
{
    public enum PaymentHistoryStatus
    {
        Pending = 1,
        Deleted = 0,
        Paid = 2
    }

    public enum PaymentHistorySortCriteria
    {
        ActualPrice,
        UsedPoint,
        EarnedPoint,
        Status
    }
}