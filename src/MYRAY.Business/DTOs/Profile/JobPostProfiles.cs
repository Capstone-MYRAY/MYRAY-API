using System.Security.Cryptography;
using AutoMapper;
using MYRAY.Business.DTOs.JobPost;
using MYRAY.Business.DTOs.Message;
using MYRAY.Business.Enums;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.DTOs.Profile;

public static class JobPostProfiles
{
    public static void ConfigJobPost(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<DataTier.Entities.JobPost, JobPostDetail>()
            .ForMember(des => des.GardenLat,
                expression => expression.MapFrom(src => src.Garden.Latitudes))
            .ForMember(des => des.GardenLon,
                expression => expression.MapFrom(src => src.Garden.Longitudes))
            .ForMember(des => des.GardenName,
                otp => otp.MapFrom(src => src.Garden.Name))
            .ForMember(des => des.WorkTypeName,
                otp => otp.MapFrom(src => src.WorkType.Name))
            .ForMember(des => des.Address,
                otp => otp.MapFrom(src => src.Garden.Address))
            .ForMember(des => des.Color, otp => otp.MapFrom(src => src.PostType.Color))
            .ForMember(des => des.Background, otp => otp.MapFrom(src => src.PostType.Background))
            .ForMember(des => des.PostTypeName, otp => otp.MapFrom(src => src.PostType.Name))
            .ForMember(des => des.ApprovedName, opt => opt.MapFrom(src=> src.ApprovedByNavigation.Fullname));
        // .ForMember(des => des.Status, 
                // map => map.MapFrom(src => Enum.GetName(typeof(JobPostEnum.JobPostStatus), src.Status!)));
        configuration.CreateMap<JobPostDetail, DataTier.Entities.JobPost>();
        configuration.CreateMap<DataTier.Entities.JobPost, CreateJobPost>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.JobPost, UpdateJobPost>().ReverseMap();

        configuration.CreateMap<AppliedJob, AppliedJobDetail>()
            .ForMember(des => des.JobPost,
                otp => otp.MapFrom(src=> src.JobPost))
           .ReverseMap();
        configuration.CreateMap<PayPerHourJob, PayPerHour>().ReverseMap();
        configuration.CreateMap<PayPerTaskJob, PayPerTask>().ReverseMap();

        configuration.CreateMap<DataTier.Entities.JobPost, MessageFarmer>()
            .ForMember(des => des.PublishedBy, opt => opt.MapFrom(src => src.PublishedByNavigation.Fullname)).ReverseMap();
    }
}