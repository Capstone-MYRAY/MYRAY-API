using AutoMapper;
using MYRAY.Business.DTOs.Attendance;
using MYRAY.Business.DTOs.SalaryTracking;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.DTOs.Profile;

public static class SalaryTrackingProfiles
{
    public static void ConfigAttendanceModule(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<DataTier.Entities.SalaryTracking, SalaryTrackingDetail>()
            .ForMember(des => des.Fullname, opt => opt.MapFrom(src => src.Account.Fullname))
            .ForMember(des => des.ImageUrl, opt => opt.MapFrom(src => src.Account.ImageUrl))
            .ForMember(des => des.PhoneNumber, opt => opt.MapFrom(src => src.Account.PhoneNumber))
            .ForMember(des => des.AppliedJobTitle, opt => opt.MapFrom(src => src.AppliedJob.JobPost.Title))
            .ReverseMap();

        configuration.CreateMap<AppliedJob, SalaryTrackingByJob>()
            .ForMember(des => des.Account, otp => otp.MapFrom(src => src.AppliedByNavigation))
            .ForMember(des => des.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(des => des.EndDate, opt => opt.MapFrom(src => src.EndDate))
            ;
    }
}