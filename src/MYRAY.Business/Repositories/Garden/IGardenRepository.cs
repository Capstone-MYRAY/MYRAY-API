namespace MYRAY.Business.Repositories.Garden;

public interface IGardenRepository
{
    IQueryable<DataTier.Entities.Garden> GetGardens();
    Task<DataTier.Entities.Garden> GetGardenById(int id);
    Task<DataTier.Entities.Garden> CreateGarden(DataTier.Entities.Garden garden);
    Task<DataTier.Entities.Garden> UpdateGarden(DataTier.Entities.Garden garden);
    Task<DataTier.Entities.Garden> DeleteGarden(int id);
}