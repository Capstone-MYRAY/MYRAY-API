using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.PaymentHistory;
using MYRAY.Business.Enums;

namespace MYRAY.Business.Services;

public interface IPaymentHistoryService
{
    ResponseDto.CollectiveResponse<PaymentHistoryDetail> GetPaymentHistory(
        SearchPaymentHistory searchPaymentHistory,
        PagingDto pagingDto,
        SortingDto<PaymentHistoryEnum.PaymentHistorySortCriteria> sortingDto, int accountId);

    Task<PaymentHistoryDetail> GetPaymentHistoryById(int id);
}