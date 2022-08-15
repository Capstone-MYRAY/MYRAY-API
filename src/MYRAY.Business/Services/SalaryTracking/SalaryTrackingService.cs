using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MYRAY.Business.DTOs.Attendance;
using MYRAY.Business.DTOs.SalaryTracking;
using MYRAY.Business.Enums;
using MYRAY.Business.Repositories.AppliedJob;
using MYRAY.Business.Repositories.JobPost;
using MYRAY.Business.Repositories.SalaryTracking;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Services.SalaryTracking;

public class SalaryTrackingService : ISalaryTrackingService
{
    private readonly IMapper _mapper;
    private readonly ISalaryTrackingRepository _salaryTrackingRepository;
    private readonly IJobPostRepository _jobPostRepository;
    private readonly IAppliedJobRepository _appliedJobRepository;

    public SalaryTrackingService(IMapper mapper, ISalaryTrackingRepository salaryTrackingRepository,
        IJobPostRepository jobPostRepository, IAppliedJobRepository appliedJobRepository)
    {
        _mapper = mapper;
        _salaryTrackingRepository = salaryTrackingRepository;
        _jobPostRepository = jobPostRepository;
        _appliedJobRepository = appliedJobRepository;
    }

    public async Task<SalaryTrackingDetail> CreateSalaryTracking(CheckAttendance attendance, int checkBy)
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
        DataTier.Entities.SalaryTracking? existedSalaryTracking =
            await _salaryTrackingRepository.GetSalaryTracking(appliedJob.Id, appliedJob.AppliedBy,
                attendance.DateAttendance.Date);
        if (existedSalaryTracking != null)
        {
            throw new Exception("You have been attended");
        }

        bool isHourJob = jobPost.Type.Equals("PayPerHourJob");
        int point = (attendance.Status == SalaryTrackingEnum.SalaryTrackingStatus.Paid
                     || attendance.Status == SalaryTrackingEnum.SalaryTrackingStatus.End)
            ? 1
            : 0;
        double salary = (double)((attendance.Status == SalaryTrackingEnum.SalaryTrackingStatus.Paid
                                  || attendance.Status == SalaryTrackingEnum.SalaryTrackingStatus.End)
            ? (isHourJob ? payPerHourJob.Salary : payPerTaskJob.Salary)
            : 0);
        DataTier.Entities.SalaryTracking newSalaryTracking = new DataTier.Entities.SalaryTracking()
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

        newSalaryTracking = await _salaryTrackingRepository.CreateSalaryTracking(newSalaryTracking);
        SalaryTrackingDetail result = _mapper.Map<SalaryTrackingDetail>(newSalaryTracking);
        return result;
    }

    public async Task<SalaryTrackingDetail> CreateDayOff(RequestDayOff requestDayOff, int accountId)
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
        DataTier.Entities.SalaryTracking existedSalaryTracking =
            await _salaryTrackingRepository.GetSalaryTrackingByDate(appliedJob.Id, appliedJob.AppliedBy, requestDayOff.DayOff);
        if (existedSalaryTracking != null)
        {
            throw new Exception("You have been request day off");
        }

        DataTier.Entities.SalaryTracking newSalaryTracking = new DataTier.Entities.SalaryTracking()
        {
            Date = requestDayOff.DayOff.Date,
            Salary = 0,
            Status = (int?)SalaryTrackingEnum.SalaryTrackingStatus.DayOff,
            AppliedJobId = appliedJob.Id,
            AccountId = accountId,
            BonusPoint = 0,
            Reason = requestDayOff.Reason,
            CreatedDate = DateTime.Now
        };

        await _salaryTrackingRepository.CreateSalaryTracking(newSalaryTracking);

        SalaryTrackingDetail salaryTrackingDetail = _mapper.Map<SalaryTrackingDetail>(newSalaryTracking);
        return salaryTrackingDetail;
    }

    public async Task<List<SalaryTrackingDetail>> GetListDayOffByJob(int farmerId, int? jobPostId)
    {
        IQueryable<DataTier.Entities.SalaryTracking> attendances =
            _salaryTrackingRepository.GetListDayOffByJob(farmerId, jobPostId);
        IQueryable<SalaryTrackingDetail> result = _mapper.ProjectTo<SalaryTrackingDetail>(attendances);
        var list = await result.ToListAsync();
        return list;
    }

    public async Task<List<SalaryTrackingDetail>> GetSalaryTrackings(int jobPostId, int accountId)
    {
        DataTier.Entities.AppliedJob appliedJob = await
            _appliedJobRepository.GetByJobAndAccount(jobPostId, accountId);
        IQueryable<DataTier.Entities.SalaryTracking> attendances = _salaryTrackingRepository.GetListSalaryTrackings(appliedJob.Id);
        IQueryable<SalaryTrackingDetail> result = _mapper.ProjectTo<SalaryTrackingDetail>(attendances);
        var list = await result.ToListAsync();
        return list;
    }

    public async Task<double?> GetTotalExpense(int jobPostId)
    {
        double? result = await _salaryTrackingRepository.GetTotalExpense(jobPostId);
        return result;
    }

    public async Task<List<SalaryTrackingByJob?>> GetSalaryTrackingByDate(int jobPostId, DateTime dateTime,
        SalaryTrackingEnum.SalaryTrackingStatus? status = null)
    {
        IQueryable<DataTier.Entities.AppliedJob> query =
            _appliedJobRepository.GetAppliedJobsExceptPending(jobPostId);
        IQueryable<SalaryTrackingByJob> map = _mapper.ProjectTo<SalaryTrackingByJob>(query);
        List<SalaryTrackingByJob> result = await map.ToListAsync();
        foreach (var attendanceByJob in result)
        {
            attendanceByJob.Attendances =
                attendanceByJob.Attendances.Where(a => a.Date.Value.Date == dateTime.Date).ToList();
        }

        if (status != null)
        {
            result = result.Where(abj =>
            {
                SalaryTrackingDetail? attendance = abj.Attendances.FirstOrDefault();
                if (attendance == null)
                {
                    return status == SalaryTrackingEnum.SalaryTrackingStatus.Unpaid;
                }

                return attendance.Status == (int?)status;
            }).ToList();
        }

        return result;
    }
}