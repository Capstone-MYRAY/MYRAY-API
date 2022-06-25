using AutoMapper;
using MYRAY.Business.DTOs.JobPost;
using MYRAY.Business.DTOs.TreeJob;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.DTOs.Profile;

public static class TreeJobProfiles
{
    public static void ConfigTreeJob(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<DataTier.Entities.TreeJob, TreeJobDetail>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.TreeJob, CreateTreeJob>().ReverseMap();
        configuration.CreateMap<DataTier.Entities.TreeJob, DeleteTreeJob>().ReverseMap();
    }
}