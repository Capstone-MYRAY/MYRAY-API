using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MYRAY.Business.DTOs.Attendance;
using MYRAY.Business.Enums;
using MYRAY.Business.Repositories.AppliedJob;
using MYRAY.Business.Repositories.Attendance;
using MYRAY.Business.Repositories.JobPost;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Services.Attendance;

public class AttendanceService : IAttendanceService
{
    private readonly IMapper _mapper;
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IJobPostRepository _jobPostRepository;
    private readonly IAppliedJobRepository _appliedJobRepository;
    public AttendanceService(IMapper mapper, IAttendanceRepository attendanceRepository, IJobPostRepository jobPostRepository, IAppliedJobRepository appliedJobRepository)
    {
        _mapper = mapper;
        _attendanceRepository = attendanceRepository;
        _jobPostRepository = jobPostRepository;
        _appliedJobRepository = appliedJobRepository;
    }


    public async Task CreateAttendance(CheckAttendance attendance, int checkBy, int accountId)
    {
        DataTier.Entities.JobPost jobPost = await _jobPostRepository.GetJobPostById(attendance.JobPostId);
        if (jobPost == null)
        {
            throw new Exception("Job post is not existed");
        }

        if (jobPost.Status == (int?)JobPostEnum.JobPostStatus.Pending)
        {
            throw new Exception("Job post is not approved");
        }
        PayPerHourJob payPerHourJob = await _jobPostRepository.GetPayPerHourJob(jobPost.Id);
        DataTier.Entities.AppliedJob appliedJob = await 
            _appliedJobRepository.GetByJobAndAccount(jobPost.Id, accountId);
        DataTier.Entities.Attendance existedAttendance = await _attendanceRepository.GetAttendance(appliedJob.Id, appliedJob.AppliedBy);
        if (existedAttendance != null)
        {
            throw new Exception("You have been attended");
        }
        DataTier.Entities.Attendance newAttendance = new DataTier.Entities.Attendance()
        {
            Date = DateTime.Now,
            Salary = payPerHourJob.Salary,
            Status = (int?)attendance.Status,
            Signature = attendance.Signature,
            AppliedJobId = appliedJob.Id,
            AccountId = checkBy,
            BonusPoint = 1
        };

       await _attendanceRepository.CreateAttendance(newAttendance);
       
    }

    public async Task<List<AttendanceDetail>> GetAttendances(int jobPostId, int accountId)
    {
        DataTier.Entities.AppliedJob appliedJob = await 
            _appliedJobRepository.GetByJobAndAccount(jobPostId, accountId);
       IQueryable<DataTier.Entities.Attendance> attendances = _attendanceRepository.GetListAttendances(appliedJob.Id);
       IQueryable<AttendanceDetail> result = _mapper.ProjectTo<AttendanceDetail>(attendances);
       var list = await result.ToListAsync();
       return list;
    }
}