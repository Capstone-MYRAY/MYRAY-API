using AutoMapper;
using MYRAY.Business.DTOs.PaymentHistory;

namespace MYRAY.Business.DTOs.Profile;

public static class PaymentHistoryProfiles
{
    public static void ConfigPaymentHistory(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<DataTier.Entities.PaymentHistory, PaymentHistoryDetail>().ReverseMap();
    }
}