namespace MYRAY.Business.Repositories.PaymentHistory;

public interface IPaymentHistoryRepository
{
    IQueryable<DataTier.Entities.PaymentHistory> GetPayments();
    Task<DataTier.Entities.PaymentHistory> GetPaymentHistoryById(int id);
    Task<DataTier.Entities.PaymentHistory> CreatePaymentHistory(DataTier.Entities.PaymentHistory paymentHistory);
    Task<DataTier.Entities.PaymentHistory> UpdatePaymentHistory(DataTier.Entities.PaymentHistory paymentHistory);
    Task<DataTier.Entities.PaymentHistory> DeletePaymentHistory(int id);
}