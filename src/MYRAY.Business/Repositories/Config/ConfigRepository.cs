using Microsoft.EntityFrameworkCore;
using MYRAY.Business.Repositories.Interface;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.Config;

public class ConfigRepository : IConfigRepository
{
    private readonly IDbContextFactory _contextFactory;
    private readonly IBaseRepository<DataTier.Entities.Config> _configRepository;

    public ConfigRepository(IDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
        _configRepository = contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.Config>()!;
    }

    public IQueryable<DataTier.Entities.Config> GetConfig()
    {
        IQueryable<DataTier.Entities.Config> query = _configRepository.Get();
        return query;
    }

    public string? GetConfigByKey(string key)
    {
        DataTier.Entities.Config? result = _configRepository.GetFirstOrDefault(c => c.Key.Equals(key));
        if (result != null)
        {
            return result.Value;
        }

        return null;
    }

    public async Task<DataTier.Entities.Config> UpdateConfig(string key, string value)
    {
        DataTier.Entities.Config? result = await _configRepository.GetFirstOrDefaultAsync(c => c.Key!.Equals(key));
        _configRepository.EntityEntry(result).State = EntityState.Modified;
        result!.Value = value;
        await _contextFactory.SaveAllAsync();
        return result;
    }
}