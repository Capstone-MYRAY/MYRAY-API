namespace MYRAY.Business.Repositories.Config;

public interface IConfigRepository
{
    IQueryable<DataTier.Entities.Config> GetConfig();
    string? GetConfigByKey(string key);
    Task<DataTier.Entities.Config> UpdateConfig(string key, string value);

}