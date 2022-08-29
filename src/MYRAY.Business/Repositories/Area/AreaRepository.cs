using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MYRAY.Business.Enums;
using MYRAY.Business.Exceptions;
using MYRAY.Business.Repositories.AreaAccount;
using MYRAY.Business.Repositories.Interface;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.Area;

public class AreaRepository : IAreaRepository
{
    private readonly IBaseRepository<DataTier.Entities.Area>? _areaRepository;
    private readonly IAreaAccountRepository _areaAccountRepository;
    private readonly IDbContextFactory _dbContextFactory;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AreaRepository"/> class.
    /// </summary>
    /// <param name="dbContextFactory">Injection of <see cref="DbContextFactory"/></param>
    public AreaRepository(IDbContextFactory dbContextFactory, IAreaAccountRepository areaAccountRepository)
    {
        _dbContextFactory = dbContextFactory;
        _areaAccountRepository = areaAccountRepository;
        _areaRepository = _dbContextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.Area>();
    }

    /// <see cref="IAreaRepository.GetAreaByIdAsync"/>
    public async Task<DataTier.Entities.Area?> GetAreaByIdAsync(int id)
    {
        DataTier.Entities.Area queryArea = await _areaRepository.Get(a => a.Id == id)
            .Include(a =>a.AreaAccounts)
            .ThenInclude(acc => acc.Account).FirstOrDefaultAsync();
        if (queryArea == null)
        {
            return null;
        }
        
        return queryArea;
    }

    /// <see cref="IAreaRepository.GetAreas"/>
    public IQueryable<DataTier.Entities.Area> GetAreas()
    {
        IQueryable<DataTier.Entities.Area> queryArea = _areaRepository.Get()
            .Include(a => a.AreaAccounts)
            .ThenInclude(b => b.Account);
        return queryArea;
    }

    /// <see cref="IAreaRepository.CreateAreaAsync"/>
    public async Task<DataTier.Entities.Area> CreateAreaAsync(DataTier.Entities.Area area, int? moderatorId)
    {
        await _areaRepository.InsertAsync(area);
        
        if (moderatorId != null)
        {
            await _areaAccountRepository.DeleteAreaAccountByArea(area.Id);
            await _areaAccountRepository.DeleteAreaAccountByAccount((int)moderatorId);

            area.AreaAccounts = new List<DataTier.Entities.AreaAccount>() { new DataTier.Entities.AreaAccount(){ AreaId = area.Id, AccountId = (int)moderatorId} };
            // await _areaAccountRepository.CreateAreaAccount(area.Id, (int)moderatorId);
        }

        await _dbContextFactory.SaveAllAsync();

        return area;
    }

    /// <see cref="IAreaRepository.UpdateAreaAsync"/>
    public async Task<DataTier.Entities.Area> UpdateAreaAsync(DataTier.Entities.Area area, int? moderatorId)
    {
        _areaRepository?.Update(area);
        
        if (moderatorId != null)
        {
            await _areaAccountRepository.DeleteAreaAccountByArea(area.Id);
            await _areaAccountRepository.DeleteAreaAccountByAccount((int)moderatorId);

            // area.AreaAccounts = new List<DataTier.Entities.AreaAccount>() { new DataTier.Entities.AreaAccount(){ AreaId = area.Id, AccountId = (int)moderatorId} };
            await _areaAccountRepository.CreateAreaAccount(area.Id, (int)moderatorId);
        }

        await _dbContextFactory.SaveAllAsync();

        return area;
    }
    
    /// <see cref="IAreaRepository.DeleteAreaAsync(Area)"/>
    public async Task<DataTier.Entities.Area> DeleteAreaAsync(DataTier.Entities.Area area)
    {
        area.Status = (int)AreaEnum.AreaStatus.Inactive;
        _areaRepository?.Update(area);

        await _dbContextFactory.SaveAllAsync();
        
        return area;
    }

    /// <see cref="IAreaRepository.DeleteAreaAsync(int)"/>
    public async Task<DataTier.Entities.Area> DeleteAreaAsync(int id)
    {
        DataTier.Entities.Area? area = await GetAreaByIdAsync(id);
        if (area == null)
        {
            throw new MException(StatusCodes.Status404NotFound, "Area is not existed or has been deleted", nameof(id));
        }
        
        return await DeleteAreaAsync(area);
    }

    public async Task DeleteAreaAccountByAccountId(int accountId, bool saveChange = false)
    {
        await _areaAccountRepository.DeleteAreaAccountByAccount(accountId, saveChange);
    }
}