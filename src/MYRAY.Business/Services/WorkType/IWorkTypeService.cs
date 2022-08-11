using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.WorkType;
using MYRAY.Business.Enums;

namespace MYRAY.Business.Services.WorkType;

public interface IWorkTypeService
{
    ResponseDto.CollectiveResponse<WorkTypeDetail> GetWorkTypes(
        SearchWorkType searchWorkType,
        PagingDto pagingDto,
        SortingDto<WorkTypeEnum.WorkTypeSortCriteria> sortingDto);
    
    Task<WorkTypeDetail> GetWorkTypeById(int id);
    Task<WorkTypeDetail> CreateWorkType(CreateWorkType workType);
    Task<WorkTypeDetail> UpdateWorkType(UpdateWorkType workType);
    Task<WorkTypeDetail> DeleteWorkType(int id);
}