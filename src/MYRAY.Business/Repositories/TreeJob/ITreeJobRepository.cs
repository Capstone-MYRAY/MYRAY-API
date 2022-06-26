using MYRAY.Business.DTOs.TreeJob;

namespace MYRAY.Business.Repositories.TreeJob;

public interface ITreeJobRepository
{
    IQueryable<DataTier.Entities.TreeJob> GetTreeJobs(int jobPostId);
    void InsertTreeJob(ICollection<CreateTreeJob> treeJobs, int jobPostId);
    void DeleteTreeJob(int jobPostId);
}