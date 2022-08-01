using MYRAY.Business.DTOs.Statistic;

namespace MYRAY.Business.Services.Statistic;

public interface IStatisticService
{
    Task<StatisticDetail> GetStatistic(int? moderatorId = null);
    Task<Dictionary<int, double>> GetStatisticByYear(int year ,int? moderatorId = null);
    Task<double> TotalMoney(int? moderatorId = null);
    Task<int> TotalJobPost(int? moderatorId = null);
    Task<int> TotalLandowner(int? moderatorId = null);
    Task<int> TotalFarmer(int? moderatorId = null);
    
}