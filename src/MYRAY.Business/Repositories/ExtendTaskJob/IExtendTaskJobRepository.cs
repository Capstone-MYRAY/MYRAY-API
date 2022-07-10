namespace MYRAY.Business.Repositories.ExtendTaskJob;

public interface IExtendTaskJobRepository
{
    IQueryable<DataTier.Entities.ExtendTaskJob> GetExtendTaskJobs(int jobPostId);
    IQueryable<DataTier.Entities.ExtendTaskJob> GetExtendTaskJobsAll();
    Task<DataTier.Entities.ExtendTaskJob> ApproveExtendTaskJobById(int id, int approvedBy);
    Task<DataTier.Entities.ExtendTaskJob> RejectExtendTaskJobById(int id, int approvedBy);
    Task<DataTier.Entities.ExtendTaskJob> CreateExtendTaskJob(DataTier.Entities.ExtendTaskJob extendTask);
    Task<DataTier.Entities.ExtendTaskJob> UpdateExtendTaskJob(DataTier.Entities.ExtendTaskJob extendTask);
    Task<DataTier.Entities.ExtendTaskJob> DeleteExtendTaskJob(int id);
}