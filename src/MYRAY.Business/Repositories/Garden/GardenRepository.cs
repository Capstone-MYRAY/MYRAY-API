using MYRAY.Business.Enums;
using MYRAY.Business.Repositories.Interface;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.Garden;

public class GardenRepository : IGardenRepository
{
    private readonly IDbContextFactory _contextFactory;
    private readonly IBaseRepository<DataTier.Entities.Garden> _gardenRepository;

    public GardenRepository(IDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
        _gardenRepository = _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.Garden>()!;
    }

    public IQueryable<DataTier.Entities.Garden> GetGardens()
    {
        IQueryable<DataTier.Entities.Garden> query =
            _gardenRepository.Get(g => g.Status == (int?)GardenEnum.GardenStatus.Active);
        return query;
    }

    public async Task<DataTier.Entities.Garden> GetGardenById(int id)
    {
        DataTier.Entities.Garden garden =
            (await _gardenRepository.GetFirstOrDefaultAsync(g =>
                g.Id == id && g.Status == (int?)GardenEnum.GardenStatus.Active))!;
        return garden;
    }

    public async Task<DataTier.Entities.Garden> CreateGarden(DataTier.Entities.Garden garden)
    {
        await _gardenRepository.InsertAsync(garden);

        await _contextFactory.SaveAllAsync();

        return garden;
    }

    public async Task<DataTier.Entities.Garden> UpdateGarden(DataTier.Entities.Garden garden)
    {
        _gardenRepository.Modify(garden);

        await _contextFactory.SaveAllAsync();

        return garden;
    }

    public async Task<DataTier.Entities.Garden> DeleteGarden(int id)
    {
        DataTier.Entities.Garden garden = _gardenRepository.GetById(id)!;
        if (garden == null || garden.Status == (int?)GardenEnum.GardenStatus.Inactive)
        {
            throw new Exception("Garden is not existed");
        }

        garden.Status = (int?)GardenEnum.GardenStatus.Inactive;
        garden.CreateDate = null;
        _gardenRepository.Modify(garden);

        await _contextFactory.SaveAllAsync();

        return garden;
    }
}