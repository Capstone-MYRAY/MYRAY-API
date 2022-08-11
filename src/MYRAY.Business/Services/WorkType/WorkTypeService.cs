using AutoMapper;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.WorkType;
using MYRAY.Business.Enums;
using MYRAY.Business.Helpers;
using MYRAY.Business.Helpers.Paging;
using MYRAY.Business.Repositories.WorkType;

namespace MYRAY.Business.Services.WorkType;

public class WorkTypeService : IWorkTypeService
{
    private readonly IMapper _mapper;
    private readonly IWorkTypeRepository _workTypeRepository;

    public WorkTypeService(IMapper mapper, IWorkTypeRepository workTypeRepository)
    {
        _mapper = mapper;
        _workTypeRepository = workTypeRepository;
    }

    public ResponseDto.CollectiveResponse<WorkTypeDetail> GetWorkTypes(SearchWorkType searchWorkType, PagingDto pagingDto, SortingDto<WorkTypeEnum.WorkTypeSortCriteria> sortingDto)
    {
        IQueryable<DataTier.Entities.WorkType> query = _workTypeRepository.GetWorkTypes();

        query = query.GetWithSearch(searchWorkType);

        query = query.GetWithSorting(sortingDto.SortColumn.ToString(), sortingDto.OrderBy);

        var result = query.GetWithPaging<WorkTypeDetail, DataTier.Entities.WorkType>(pagingDto, _mapper);

        return result;
    }

    public async Task<WorkTypeDetail> GetWorkTypeById(int id)
    {
        DataTier.Entities.WorkType workType = await _workTypeRepository.GetWorkTypeById(id);
        var result = _mapper.Map<WorkTypeDetail>(workType);
        return result;
    }

    public async Task<WorkTypeDetail> CreateWorkType(CreateWorkType workType)
    {
        DataTier.Entities.WorkType newWorkType = _mapper.Map<DataTier.Entities.WorkType>(workType);
        newWorkType.Status = (int?)WorkTypeEnum.WorkTypeStatus.Active;

        newWorkType = await _workTypeRepository.CreateWorkType(newWorkType);

        var result = _mapper.Map<WorkTypeDetail>(newWorkType);
        return result;
    }

    public async Task<WorkTypeDetail> UpdateWorkType(UpdateWorkType workType)
    {
        DataTier.Entities.WorkType updateWorkType = _mapper.Map<DataTier.Entities.WorkType>(workType);
        updateWorkType.Status = (int?)WorkTypeEnum.WorkTypeStatus.Active;

        updateWorkType = await _workTypeRepository.UpdateWorkType(updateWorkType);

        var result = _mapper.Map<WorkTypeDetail>(updateWorkType);
        return result;
    }

    public async Task<WorkTypeDetail> DeleteWorkType(int id)
    {
        DataTier.Entities.WorkType workType = await _workTypeRepository.DeleteWorkType(id);
        var result = _mapper.Map<WorkTypeDetail>(workType);
        return result;
    }
}