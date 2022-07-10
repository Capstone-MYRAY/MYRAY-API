using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.ExtendTaskJob;
using MYRAY.Business.DTOs.JobPost;
using MYRAY.Business.Enums;

namespace MYRAY.Business.Services.ExtendTaskJob;

public interface IExtendTaskJobService
{
    Task<ExtendTaskJobDetail> CheckOneExtend(int jobPostId);
    
    ResponseDto.CollectiveResponse<ExtendTaskJobDetail> GetExtendTaskJobsALl(
        SearchExtendRequest searchExtendRequest,
        PagingDto pagingDto,
        SortingDto<ExtendTaskJobEnum.SortCriteriaExtendTaskJob> sortingDto);

    Task<ExtendTaskJobDetail> CreateExtendTaskJob(CreateExtendRequest extendRequest, int requestBy);
    Task<ExtendTaskJobDetail> UpdateExtendTaskJob(UpdateExtendRequest extendRequest, int requestBy);

    Task<ExtendTaskJobDetail> DeleteExtendTaskJob(int id);
    Task<ExtendTaskJobDetail> ApproveExtendTaskJob(int eTJId, int approvedBy);
    Task<ExtendTaskJobDetail> RejectExtendTaskJob(int eTJId, int approvedBy);

}