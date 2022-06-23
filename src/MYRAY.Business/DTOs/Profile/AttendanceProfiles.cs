using AutoMapper;
using MYRAY.Business.DTOs.Attendance;

namespace MYRAY.Business.DTOs.Profile;

public static class AttendanceProfiles
{
    public static void ConfigAttendanceModule(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<DataTier.Entities.Attendance, AttendanceDetail>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.Attendance, CheckAttendance>().ReverseMap();
    }
}