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

    public AttendanceService(IMapper mapper, IAttendanceRepository attendanceRepository,
        IJobPostRepository jobPostRepository, IAppliedJobRepository appliedJobRepository)
    {
        _mapper = mapper;
        _attendanceRepository = attendanceRepository;
        _jobPostRepository = jobPostRepository;
        _appliedJobRepository = appliedJobRepository;
    }

    public async Task<AttendanceDetail> CreateAttendance(CheckAttendance attendance, int checkBy)
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

        if (jobPost.Type.Equals("PayPerHourJob"))
        {
            
        }
        PayPerHourJob? payPerHourJob = await _jobPostRepository.GetPayPerHourJob(jobPost.Id);
        PayPerTaskJob? payPerTaskJob = await _jobPostRepository.GetPayPerTaskJob(jobPost.Id);
        DataTier.Entities.AppliedJob appliedJob = await
            _appliedJobRepository.GetByJobAndAccount(jobPost.Id, attendance.AccountId);
        DataTier.Entities.Attendance? existedAttendance =
            await _attendanceRepository.GetAttendance(appliedJob.Id, appliedJob.AppliedBy,
                attendance.DateAttendance.Date);
        if (existedAttendance != null)
        {
            throw new Exception("You have been attended");
        }

        bool isHourJob = jobPost.Type.Equals("PayPerHourJob");
        int point = (attendance.Status == AttendanceEnum.AttendanceStatus.Present
                     || attendance.Status == AttendanceEnum.AttendanceStatus.End)
            ? 1
            : 0;
        double salary = (double)((attendance.Status == AttendanceEnum.AttendanceStatus.Present
                                  || attendance.Status == AttendanceEnum.AttendanceStatus.End)
            ? (isHourJob ? payPerHourJob.Salary : payPerTaskJob.Salary)
            : 0);
        DataTier.Entities.Attendance newAttendance = new DataTier.Entities.Attendance()
        {
            Date = attendance.DateAttendance,
            Salary = salary,
            Status = (int?)attendance.Status,
            Signature = attendance.Signature,
            AppliedJobId = appliedJob.Id,
            AccountId = attendance.AccountId,
            BonusPoint = point,
            Reason = attendance.Reason,
            CreatedDate = DateTime.Now
        };

        newAttendance = await _attendanceRepository.CreateAttendance(newAttendance);
        AttendanceDetail result = _mapper.Map<AttendanceDetail>(newAttendance);
        return result;
    }

    public async Task<AttendanceDetail> CreateDayOff(RequestDayOff requestDayOff, int accountId)
    {
        DataTier.Entities.JobPost jobPost = await _jobPostRepository.GetJobPostById(requestDayOff.JobPostId);
        if (jobPost == null)
        {
            throw new Exception("Job post is not existed");
        }

        if (!jobPost.Type.Equals("PayPerHourJob"))
        {
            throw new Exception("Day Off just for hour job");
        }

        if (jobPost.StatusWork != (int?)JobPostEnum.JobPostWorkStatus.Started)
        {
            throw new Exception("Job post is not started");
        }

        PayPerHourJob payPerHourJob = await _jobPostRepository.GetPayPerHourJob(jobPost.Id);
        if (requestDayOff.DayOff.Date.AddHours(payPerHourJob.StartTime.Value.Hours) < DateTime.Now.AddHours(24))
        {
            throw new Exception("Day-off must from the current 24hour");
        }

        DataTier.Entities.AppliedJob appliedJob = await
            _appliedJobRepository.GetByJobAndAccount(jobPost.Id, accountId);
        DataTier.Entities.Attendance existedAttendance =
            await _attendanceRepository.GetAttendanceByDate(appliedJob.Id, appliedJob.AppliedBy, requestDayOff.DayOff);
        if (existedAttendance != null)
        {
            throw new Exception("You have been request day off");
        }

        DataTier.Entities.Attendance newAttendance = new DataTier.Entities.Attendance()
        {
            Date = requestDayOff.DayOff.Date,
            Salary = 0,
            Status = (int?)AttendanceEnum.AttendanceStatus.DayOff,
            AppliedJobId = appliedJob.Id,
            AccountId = accountId,
            BonusPoint = 0,
            Reason = requestDayOff.Reason,
            CreatedDate = DateTime.Now
        };

        await _attendanceRepository.CreateAttendance(newAttendance);

        AttendanceDetail attendanceDetail = _mapper.Map<AttendanceDetail>(newAttendance);
        return attendanceDetail;
    }

    public async Task<List<AttendanceDetail>> GetListDayOffByJob(int farmerId, int? jobPostId)
    {
        IQueryable<DataTier.Entities.Attendance> attendances =
            _attendanceRepository.GetListDayOffByJob(farmerId, jobPostId);
        IQueryable<AttendanceDetail> result = _mapper.ProjectTo<AttendanceDetail>(attendances);
        var list = await result.ToListAsync();
        return list;
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

    public async Task<double?> GetTotalExpense(int jobPostId)
    {
        double? result = await _attendanceRepository.GetTotalExpense(jobPostId);
        return result;
    }

    public async Task<List<AttendanceByJob?>> GetAttendanceByDate(int jobPostId, DateTime dateTime,
        AttendanceEnum.AttendanceStatus? status = null)
    {
        IQueryable<DataTier.Entities.AppliedJob> query =
            _appliedJobRepository.GetAppliedJobsExceptPending(jobPostId);
        IQueryable<AttendanceByJob> map = _mapper.ProjectTo<AttendanceByJob>(query);
        List<AttendanceByJob> result = await map.ToListAsync();
        foreach (var attendanceByJob in result)
        {
            attendanceByJob.Attendances =
                attendanceByJob.Attendances.Where(a => a.Date.Value.Date == dateTime.Date).ToList();
        }

        if (status != null)
        {
            result = result.Where(abj =>
            {
                AttendanceDetail? attendance = abj.Attendances.FirstOrDefault();
                if (attendance == null)
                {
                    return status == AttendanceEnum.AttendanceStatus.Future;
                }

                return attendance.Status == (int?)status;
            }).ToList();
        }

        return result;
    }
}