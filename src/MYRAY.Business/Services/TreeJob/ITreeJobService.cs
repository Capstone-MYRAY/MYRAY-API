using MYRAY.Business.DTOs.TreeJob;

namespace MYRAY.Business.Services.TreeJob;

public interface ITreeJobService
{
    IEnumerable<TreeJobDetail> GetTreeJobs(int jobPostId);
    
    Task<TreeJobDetail> CreateTreeJobs(CreateTreeJob treeJob);
    Task<TreeJobDetail> DeleteTreeJobs(DeleteTreeJob treeJob);
}