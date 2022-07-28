namespace MYRAY.Business.Services.Statistic;

public interface IStatisticService
{
    Task<double> TotalMoney(int? moderatorId = null);
    Task<int> TotalJobPost(int? moderatorId = null);
    Task<int> TotalLandowner(int? moderatorId = null);
    Task<int> TotalFarmer(int? moderatorId = null);
    
}