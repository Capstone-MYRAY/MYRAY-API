using System.Security.Cryptography;
using AutoMapper;
using MYRAY.Business.DTOs.JobPost;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.DTOs.Profile;

public static class JobPostProfiles
{
    public static void ConfigJobPost(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<DataTier.Entities.JobPost, JobPostDetail>()
            .ForMember(des => des.PublishedName,
                expression => expression.MapFrom(src => src.PublishedByNavigation!.Fullname))
            .ForMember(des => des.GardenName,
                otp => otp.MapFrom(src => src.Garden.Name));
        configuration.CreateMap<JobPostDetail, DataTier.Entities.JobPost>();
        configuration.CreateMap<DataTier.Entities.JobPost, CreateJobPost>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.JobPost, UpdateJobPost>().ReverseMap();

        configuration.CreateMap<AppliedJob, AppliedJobDetail>().ReverseMap();
        configuration.CreateMap<PayPerHourJob, PayPerHour>().ReverseMap();
        configuration.CreateMap<PayPerTaskJob, PayPerTask>().ReverseMap();
    }
}