using MYRAY.Business.DTOs.Statistic;

namespace MYRAY.Business.Repositories.Statistic;

public interface IStatisticRepository
{
    Task<DataTier.Entities.Area> GetAreaByModeratorId(int moderatorId);
    Task<double> TotalMoney(int? areaId = null);
    Task<Dictionary<int, double>> GetTotalMoneyByYear(int year);
    IQueryable<DataTier.Entities.JobPost> TotalJobPosts(int? areaId = null);
    IQueryable<DataTier.Entities.Account> TotalLandowner(int? areaId = null);
    IQueryable<DataTier.Entities.Account> TotalFarmer(int? areaId = null);
}