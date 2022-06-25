using AutoMapper;
using MYRAY.Business.DTOs.TreeJob;
using MYRAY.Business.Repositories.TreeJob;

namespace MYRAY.Business.Services.TreeJob;

public class TreeJobService : ITreeJobService
{
    private IMapper _mapper;
    private ITreeJobRepository _treeJobRepository;

    public TreeJobService(IMapper mapper, ITreeJobRepository treeJobRepository)
    {
        _mapper = mapper;
        _treeJobRepository = treeJobRepository;
    }

    public IEnumerable<TreeJobDetail> GetTreeJobs(int jobPostId)
    {
        IQueryable<DataTier.Entities.TreeJob> query = _treeJobRepository.GetTreeJobs(jobPostId);
        IEnumerable<TreeJobDetail> result = _mapper.ProjectTo<TreeJobDetail>(query);
        return result;
    }

    public async Task<TreeJobDetail> CreateTreeJobs(CreateTreeJob treeJob)
    {
        DataTier.Entities.TreeJob treeJobDto = _mapper.Map<DataTier.Entities.TreeJob>(treeJob);
        try
        {
            treeJobDto = await _treeJobRepository.CreateTreeJob(treeJobDto);
        }
        catch (Exception e)
        {
            throw new Exception("TreeJob is existed");
        }
        TreeJobDetail result = _mapper.Map<TreeJobDetail>(treeJobDto);
        return result;

    }

    public async Task<TreeJobDetail> DeleteTreeJobs(DeleteTreeJob treeJob)
    {
        DataTier.Entities.TreeJob treeJobDto = _mapper.Map<DataTier.Entities.TreeJob>(treeJob);
        treeJobDto = await _treeJobRepository.DeleteTreeJob(treeJobDto);
        TreeJobDetail result = _mapper.Map<TreeJobDetail>(treeJobDto);
        return result;
    }
}