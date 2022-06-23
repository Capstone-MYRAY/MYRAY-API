using AutoMapper.Execution;
using Microsoft.AspNetCore.Http;
using MYRAY.Business.Enums;
using MYRAY.Business.Exceptions;
using MYRAY.Business.Repositories.Interface;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.PaymentHistory;

public class PaymentHistoryRepository : IPaymentHistoryRepository
{
    private readonly IDbContextFactory _contextFactory;
    private readonly IBaseRepository<DataTier.Entities.PaymentHistory> _paymentHistoryRepository;

    public PaymentHistoryRepository(IDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
        _paymentHistoryRepository =
            _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.PaymentHistory>()!;
    }

    public IQueryable<DataTier.Entities.PaymentHistory> GetPayments(int accountId)
    {
        IQueryable<DataTier.Entities.PaymentHistory> query = _paymentHistoryRepository.Get(ph => ph.BelongedId == accountId);
        return query;
    }

    public async Task<DataTier.Entities.PaymentHistory> GetPaymentHistoryById(int id)
    {
        DataTier.Entities.PaymentHistory result = (await _paymentHistoryRepository.GetFirstOrDefaultAsync(ph => ph.Id == id))!;
        return result;
    }

    public async Task<DataTier.Entities.PaymentHistory> CreatePaymentHistory(DataTier.Entities.PaymentHistory paymentHistory)
    {
        await _paymentHistoryRepository.InsertAsync(paymentHistory);

        await _contextFactory.SaveAllAsync();
        return paymentHistory;
    }

    public async Task<DataTier.Entities.PaymentHistory> UpdatePaymentHistory(DataTier.Entities.PaymentHistory paymentHistory)
    {
         _paymentHistoryRepository.Modify(paymentHistory);

        await _contextFactory.SaveAllAsync();
        return paymentHistory;
    }

    public async Task<DataTier.Entities.PaymentHistory> DeletePaymentHistory(int id)
    {
        DataTier.Entities.PaymentHistory? payment = await _paymentHistoryRepository.GetByIdAsync(id);

        if (payment == null)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Payment is not existed.");
        }

        payment.Status = (int?)PaymentHistoryEnum.PaymentHistoryStatus.Reject;
        
        _paymentHistoryRepository.Modify(payment);

        await _contextFactory.SaveAllAsync();

        return payment;
    }
}