using MYRAY.Business.DTOs.TreeJob;
using MYRAY.Business.Repositories.Interface;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.TreeJob;

public class TreeJobRepository : ITreeJobRepository
{
    private readonly IDbContextFactory _contextFactory;
    private readonly IBaseRepository<DataTier.Entities.TreeJob> _treeJobRepository;

    public TreeJobRepository(IDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
        _treeJobRepository = _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.TreeJob>()!;
    }


    public IQueryable<DataTier.Entities.TreeJob> GetTreeJobs(int jobPostId)
    {
        IQueryable<DataTier.Entities.TreeJob> query = _treeJobRepository.Get(tj => tj.JobPostId == jobPostId);
        return query;
    }

    public void InsertTreeJob(ICollection<CreateTreeJob> treeJobs, int jobPostId)
    {
        foreach (var tf in treeJobs)
        {
             _treeJobRepository.Insert(new DataTier.Entities.TreeJob
             {
                TreeTypeId = tf.TreeTypeId,
                JobPostId = jobPostId,
                CreatedDate = DateTime.Now
            });
        }
    }

    public void DeleteTreeJob(int jobPostId)
    {
        IQueryable<DataTier.Entities.TreeJob> queryable = _treeJobRepository.Get(tj => tj.JobPostId == jobPostId);
        List<DataTier.Entities.TreeJob> listDelete = queryable.ToList();
        foreach (var tf in listDelete)
        {
            _treeJobRepository.Delete(tf);
        }
    }
}