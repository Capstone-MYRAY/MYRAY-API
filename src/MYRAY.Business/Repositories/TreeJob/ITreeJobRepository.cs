namespace MYRAY.Business.Repositories.TreeJob;

public interface ITreeJobRepository
{
    IQueryable<DataTier.Entities.TreeJob> GetTreeJobs(int jobPostId);
    
    Task<DataTier.Entities.TreeJob> CreateTreeJob(DataTier.Entities.TreeJob treeJob);
    Task<DataTier.Entities.TreeJob> DeleteTreeJob(DataTier.Entities.TreeJob treeJob);
}