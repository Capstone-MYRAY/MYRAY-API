using AutoMapper;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.PaymentHistory;
using MYRAY.Business.Enums;
using MYRAY.Business.Helpers;
using MYRAY.Business.Helpers.Paging;
using MYRAY.Business.Repositories.PaymentHistory;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Services;

public class PaymentHistoryService : IPaymentHistoryService
{
    private readonly IMapper _mapper;
    private readonly IPaymentHistoryRepository _paymentHistoryRepository;

    public PaymentHistoryService(IMapper mapper, IPaymentHistoryRepository paymentHistoryRepository)
    {
        _mapper = mapper;
        _paymentHistoryRepository = paymentHistoryRepository;
    }

    public ResponseDto.CollectiveResponse<PaymentHistoryDetail> GetPaymentHistory(
        SearchPaymentHistory searchPaymentHistory, 
        PagingDto pagingDto, 
        SortingDto<PaymentHistoryEnum.PaymentHistorySortCriteria> sortingDto,
        int accountId)
    {
        IQueryable<PaymentHistory> query = _paymentHistoryRepository.GetPayments(accountId);

        query = query.GetWithSearch(searchPaymentHistory);
        query = query.Where(p =>
            searchPaymentHistory.MinDate == null || p.CreatedDate.Date >= searchPaymentHistory.MinDate);
        
        query = query.Where(p =>
            searchPaymentHistory.MaxDate == null || p.CreatedDate.Date <= searchPaymentHistory.MaxDate);

        query = query.GetWithSorting(sortingDto.SortColumn.ToString(), sortingDto.OrderBy);

        var result = query.GetWithPaging<PaymentHistoryDetail, PaymentHistory>(pagingDto, _mapper);

        return result;
    }

    public async Task<PaymentHistoryDetail> GetPaymentHistoryById(int id)
    {
        PaymentHistory? paymentHistory = await _paymentHistoryRepository.GetPaymentHistoryById(id);
        PaymentHistoryDetail result = _mapper.Map<PaymentHistoryDetail>(paymentHistory);
        return result;
    }
}