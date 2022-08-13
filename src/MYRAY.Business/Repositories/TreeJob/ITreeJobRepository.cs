using MYRAY.Business.DTOs.TreeJob;

namespace MYRAY.Business.Repositories.TreeJob;

public interface ITreeJobRepository
{
    IQueryable<DataTier.Entities.TreeJob> GetTreeJobs(int jobPostId);
    IQueryable<DataTier.Entities.TreeJob> GetTreeJobs();
    void InsertTreeJob(ICollection<CreateTreeJob> treeJobs, int jobPostId);
    void DeleteTreeJob(int jobPostId);
}