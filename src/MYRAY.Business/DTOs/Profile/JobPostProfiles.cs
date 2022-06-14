using AutoMapper;
using MYRAY.Business.DTOs.JobPost;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.DTOs.Profile;

public static class JobPostProfiles
{
    public static void ConfigJobPost(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<DataTier.Entities.JobPost, JobPostDetail>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.JobPost, CreateJobPost>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.JobPost, UpdateJobPost>().ReverseMap();

        configuration.CreateMap<AppliedJob, AppliedJobDetail>().ReverseMap();
        configuration.CreateMap<PayPerHourJob, PayPerHour>().ReverseMap();
        configuration.CreateMap<PayPerTaskJob, PayPerTask>().ReverseMap();
    }
}