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

    public async Task<DataTier.Entities.TreeJob> CreateTreeJob(DataTier.Entities.TreeJob treeJob)
    {
        await _treeJobRepository.InsertAsync(treeJob);

        await _contextFactory.SaveAllAsync();
        return treeJob;
    }

    public async Task<DataTier.Entities.TreeJob> DeleteTreeJob(DataTier.Entities.TreeJob treeJob)
    {
        DataTier.Entities.TreeJob treeJobDb = await _treeJobRepository.GetFirstOrDefaultAsync(tj =>
            tj.JobPostId == treeJob.JobPostId && tj.TreeTypeId == treeJob.TreeTypeId) ?? throw new Exception("TreJob is not existed");

        if (treeJobDb == null)
        {
            throw new Exception("TreeJob is not existed");
        }

        _treeJobRepository.Delete(treeJobDb);

        await _contextFactory.SaveAllAsync();
        return treeJob;
    }
}