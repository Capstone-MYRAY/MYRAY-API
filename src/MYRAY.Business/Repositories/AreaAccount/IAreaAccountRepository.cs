namespace MYRAY.Business.Repositories.AreaAccount;

public interface IAreaAccountRepository
{
    IQueryable<DataTier.Entities.AreaAccount> GetAreaAccount();
    
    Task<DataTier.Entities.AreaAccount> CreateAreaAccount(int areaId, int accountId);
    Task DeleteAreaAccountByAccount(int accountId, bool saveChange = false);
    Task DeleteAreaAccountByArea(int areaId);
}