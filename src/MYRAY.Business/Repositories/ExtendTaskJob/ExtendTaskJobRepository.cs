using MYRAY.Business.Enums;
using MYRAY.Business.Repositories.Interface;
using MYRAY.Business.Services.Notification;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.ExtendTaskJob;

public class ExtendTaskJobRepository : IExtendTaskJobRepository
{
    private readonly IDbContextFactory _contextFactory;
    private readonly IBaseRepository<DataTier.Entities.ExtendTaskJob> _extendTaskJobRepository;
    private readonly IBaseRepository<DataTier.Entities.JobPost> _jobPostRepository;
    
    public ExtendTaskJobRepository(IDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
        _jobPostRepository =  _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.JobPost>()!;
        _extendTaskJobRepository =
            _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.ExtendTaskJob>()!;
    }


    public IQueryable<DataTier.Entities.ExtendTaskJob> GetExtendTaskJobs(int jobPostId)
    {
        IQueryable<DataTier.Entities.ExtendTaskJob> query =
            _extendTaskJobRepository.Get(etj => etj.JobPostId == jobPostId);
        return query;
    }

    public IQueryable<DataTier.Entities.ExtendTaskJob> GetExtendTaskJobsAll(int? accountId = null)
    {
        IQueryable<DataTier.Entities.ExtendTaskJob> query =
            _extendTaskJobRepository.Get(etj => accountId == null || etj.JobPost.PublishedBy == accountId);
        return query;
    }

    public async Task<DataTier.Entities.ExtendTaskJob> ApproveExtendTaskJobById(int id, int approvedBy)
    {
        DataTier.Entities.ExtendTaskJob? extendTaskJob = await 
            _extendTaskJobRepository.GetFirstOrDefaultAsync(etj => etj.Id == id);
        if (extendTaskJob == null)
        {
            throw new Exception("No extend task job existed");
        }
        
        if (extendTaskJob.Status != (int?)ExtendTaskJobEnum.ExtendTaskJobStatus.Pending)
        {
            throw new Exception("Request is not pending");
        }

        DataTier.Entities.JobPost jobPost = (await _jobPostRepository.GetFirstOrDefaultAsync(j => j.Id == extendTaskJob.JobPostId))!;
        jobPost.EndJobDate = extendTaskJob.ExtendEndDate;

        extendTaskJob.ApprovedBy = approvedBy;
        extendTaskJob.ApprovedDate = DateTime.Now;
        extendTaskJob.Status = (int?)ExtendTaskJobEnum.ExtendTaskJobStatus.Approve;
        _extendTaskJobRepository.Modify(extendTaskJob);
        _jobPostRepository.Modify(jobPost);

        await _contextFactory.SaveAllAsync();
        
        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            { "type", "extendJob" },
            { "action", "approved"},
            {"jobPostId" , jobPost.Id.ToString()}
        };
        await PushNotification.SendMessage(extendTaskJob.RequestBy.ToString()!
            , $"Yêu cầu gia hạn của bạn đã được tiếp nhân",
            $"Bạn đã gia hạn ngày kết thúc cho công việc {jobPost.Title}", data);


        return extendTaskJob;

    }

    public async Task<DataTier.Entities.ExtendTaskJob> RejectExtendTaskJobById(int id, int approvedBy)
    {
        DataTier.Entities.ExtendTaskJob? extendTaskJob = await 
            _extendTaskJobRepository.GetFirstOrDefaultAsync(etj => etj.Id == id);
        if (extendTaskJob == null)
        {
            throw new Exception("No extend task job existed");
        }

        if (extendTaskJob.Status != (int?)ExtendTaskJobEnum.ExtendTaskJobStatus.Pending)
        {
            throw new Exception("Request is not pending");
        }

        extendTaskJob.ApprovedBy = approvedBy;
        extendTaskJob.ApprovedDate = DateTime.Now;
        extendTaskJob.Status = (int?)ExtendTaskJobEnum.ExtendTaskJobStatus.Reject;
        _extendTaskJobRepository.Modify(extendTaskJob);
        
        await _contextFactory.SaveAllAsync();
        DataTier.Entities.JobPost jobPost = _jobPostRepository.GetById(extendTaskJob.JobPostId);
        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            { "type", "extendJob" },
            {"jobPostId" , jobPost!.Id.ToString()}
        };
        await PushNotification.SendMessage(extendTaskJob.RequestBy.ToString()!
            , $"Yêu cầu gia hạn của bạn đã bị từ chối.",
            $"Bạn đã không được gia hạn ngày kết thúc cho công việc {jobPost.Title}", data);
        
        return extendTaskJob;
    }

    public async Task<DataTier.Entities.ExtendTaskJob> CreateExtendTaskJob(DataTier.Entities.ExtendTaskJob extendTask)
    {
        extendTask.CreatedDate = DateTime.Now;
        DataTier.Entities.JobPost jobPost = _jobPostRepository.GetById(extendTask.JobPostId);
        extendTask.OldEndDate = jobPost.EndJobDate;
        await _extendTaskJobRepository.InsertAsync(extendTask);
        await _contextFactory.SaveAllAsync();

        return extendTask;
    }

    public async Task<DataTier.Entities.ExtendTaskJob> UpdateExtendTaskJob(DataTier.Entities.ExtendTaskJob extendTask)
    {
        _extendTaskJobRepository.Modify(extendTask);

        await _contextFactory.SaveAllAsync();
        return extendTask;
    }

    public async Task<DataTier.Entities.ExtendTaskJob> DeleteExtendTaskJob(int id)
    {
        DataTier.Entities.ExtendTaskJob? extendTaskJob = await _extendTaskJobRepository.GetByIdAsync(id);
        if (extendTaskJob == null)
        {
            throw new Exception("No extend task job existed");
        }
        _extendTaskJobRepository.Delete(extendTaskJob);
        await _contextFactory.SaveAllAsync();

        return extendTaskJob;
    }
}