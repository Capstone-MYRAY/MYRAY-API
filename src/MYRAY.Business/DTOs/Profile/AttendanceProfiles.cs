using AutoMapper;
using MYRAY.Business.DTOs.Attendance;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.DTOs.Profile;

public static class AttendanceProfiles
{
    public static void ConfigAttendanceModule(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<DataTier.Entities.Attendance, AttendanceDetail>().ReverseMap();

        configuration.CreateMap<AppliedJob, AttendanceByJob>()
            .ForMember(des => des.Account, otp => otp.MapFrom(src => src.AppliedByNavigation));
    }
}