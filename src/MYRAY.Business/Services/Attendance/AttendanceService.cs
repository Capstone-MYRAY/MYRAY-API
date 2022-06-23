using AutoMapper;
using MYRAY.Business.DTOs.Attendance;
using MYRAY.Business.Repositories.Attendance;
using MYRAY.Business.Repositories.JobPost;

namespace MYRAY.Business.Services.Attendance;

public class AttendanceService : IAttendanceService
{
    private readonly IMapper _mapper;
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IJobPostRepository _jobPostRepository;

    public AttendanceService(IMapper mapper, IAttendanceRepository attendanceRepository)
    {
        _mapper = mapper;
        _attendanceRepository = attendanceRepository;
    }


    public async Task CreateAttendance()
    {
        IQueryable<DataTier.Entities.JobPost> jobPostInProgress = _jobPostRepository.GetInProgressJobPost();
        
    }

    public async Task CreateAttendanceManual(int jobPostId)
    {
        DataTier.Entities.JobPost jobPosts = await _jobPostRepository.GetJobPostById(jobPostId);
        ICollection<DataTier.Entities.AppliedJob> appliedJobs = jobPosts.AppliedJobs;
        
        
    }

    public Task<AttendanceDetail> CheckAttendance()
    {
        return null;
    }
}