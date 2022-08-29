using Microsoft.EntityFrameworkCore;
using MYRAY.Business.Repositories.Interface;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.AreaAccount;

public class AreaAccountRepository : IAreaAccountRepository
{
    private readonly IDbContextFactory _contextFactory;
    private readonly IBaseRepository<DataTier.Entities.AreaAccount> _areaAccountRepository;

    public AreaAccountRepository(IDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
        _areaAccountRepository =
            _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.AreaAccount>();
    }

    public IQueryable<DataTier.Entities.AreaAccount> GetAreaAccount()
    {
        IQueryable<DataTier.Entities.AreaAccount> query = _areaAccountRepository.Get();
        return query;
    }

    public async Task<DataTier.Entities.AreaAccount> CreateAreaAccount(int areaId, int accountId)
    {
        DataTier.Entities.AreaAccount areaAccount = new DataTier.Entities.AreaAccount()
        {
            AreaId = areaId,
            AccountId = accountId,
            CreatedDate = DateTime.Now
        };
        await _areaAccountRepository.InsertAsync(areaAccount);

        // await _contextFactory.SaveAllAsync();

        return areaAccount;
    }


    public async Task DeleteAreaAccountByAccount(int accountId, bool saveChange = false)
    {
        IQueryable<DataTier.Entities.AreaAccount> query = _areaAccountRepository.Get(aa => aa.AccountId == accountId);
        List<DataTier.Entities.AreaAccount> list = await query.ToListAsync();
        if (list.Count > 0)
        {
            foreach (var areaAccount in list)
            {
                _areaAccountRepository.Delete(areaAccount);
            }
        }

        if (saveChange)
            await _contextFactory.SaveAllAsync();
    }

    public async Task DeleteAreaAccountByArea(int areaId)
    {
        IQueryable<DataTier.Entities.AreaAccount> query = _areaAccountRepository.Get(aa => aa.AreaId == areaId);
        List<DataTier.Entities.AreaAccount> list = await query.ToListAsync();
        if (list.Count > 0)
        {
            foreach (var VARIABLE in list)
            {
                _areaAccountRepository.Delete(VARIABLE);
            }
        }

        // await _contextFactory.SaveAllAsync();
    }
}