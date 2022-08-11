namespace MYRAY.Business.Repositories.WorkType;

public interface IWorkTypeRepository
{
    IQueryable<DataTier.Entities.WorkType> GetWorkTypes();
    Task<DataTier.Entities.WorkType?> GetWorkTypeById(int id);
    Task<DataTier.Entities.WorkType?> CreateWorkType(DataTier.Entities.WorkType workType);
    Task<DataTier.Entities.WorkType?> UpdateWorkType(DataTier.Entities.WorkType workType);
    Task<DataTier.Entities.WorkType?> DeleteWorkType(int id);
    
}