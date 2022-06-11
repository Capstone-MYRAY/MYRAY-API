using AutoMapper;
using Microsoft.AspNetCore.Http;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Area;
using MYRAY.Business.Enums;
using MYRAY.Business.Exceptions;
using MYRAY.Business.Helpers;
using MYRAY.Business.Helpers.Paging;
using MYRAY.Business.Repositories.Area;

namespace MYRAY.Business.Services.Area;
/// <summary>
/// Area Service class
/// </summary>
public class AreaService : IAreaService
{
    private readonly IMapper _mapper;
    private readonly IAreaRepository _areaRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="AreaService"/> class.
    /// </summary>
    /// <param name="mapper">Injection of <see cref="IMapper"/></param>
    /// <param name="areaRepository">Injection of <see cref="IAreaRepository"/></param>
    public AreaService(IMapper mapper, IAreaRepository areaRepository)
    {
        _mapper = mapper;
        _areaRepository = areaRepository;
    }

    /// <inheritdoc cref="IAreaService.GetAreas"/>
    public  ResponseDto.CollectiveResponse<GetAreaDetail> GetAreas(PagingDto pagingDto, SortingDto<AreaEnum.AreaSortCriteria> sortingDto, SearchAreaDto searchAreaDto)
    {
        IQueryable<DataTier.Entities.Area> queryArea = _areaRepository.GetAreas();
       
        //--Apply Search
        queryArea = queryArea.GetWithSearch(searchAreaDto);
        
        // Apply Sorting
        queryArea = queryArea.GetWithSorting(sortingDto.SortColumn.ToString(), sortingDto.OrderBy);
        
        //--Apply Paging
        ResponseDto.CollectiveResponse<GetAreaDetail> result =
            queryArea.GetWithPaging<GetAreaDetail, DataTier.Entities.Area>(pagingDto, _mapper);

        return result;
    }

    /// <inheritdoc cref="IAreaService.GetAreaByIdAsync"/>
    public async Task<GetAreaDetail> GetAreaByIdAsync(int? id)
    {
        if (id == null)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Input Id to get", nameof(GetAreaByIdAsync));
        }
        int idRole;
        if (!int.TryParse(id.ToString(), out idRole))
        {
            throw new MException(StatusCodes.Status400BadRequest, "ID must be Number", nameof(GetAreaByIdAsync));
        }

        DataTier.Entities.Area? queryArea = await _areaRepository.GetAreaByIdAsync(idRole);

        GetAreaDetail areaDto = _mapper.Map<GetAreaDetail>(queryArea);

        return areaDto;
    }

    /// <inheritdoc cref="IAreaService.CreateAreaAsync"/>
    public async Task<GetAreaDetail> CreateAreaAsync(InsertAreaDto? bodyDto)
    {
        if (bodyDto is null)
        {
            throw new MException(StatusCodes.Status400BadRequest, $"{nameof(InsertAreaDto)} is Null", nameof(bodyDto));
        }

        DataTier.Entities.Area newArea = _mapper.Map<DataTier.Entities.Area>(bodyDto);
        newArea = await _areaRepository.CreateAreaAsync(newArea);

        GetAreaDetail newAreaDto = _mapper.Map<GetAreaDetail>(newArea);

        return newAreaDto;
    }

    /// <inheritdoc cref="IAreaService.UpdateAreaAsync"/>
    public async Task<UpdateAreaDto> UpdateAreaAsync(UpdateAreaDto bodyDto)
    {
        UpdateAreaDto updateAreaDto;
        try
        {
            if (bodyDto is null)
            {
                throw new MException(StatusCodes.Status400BadRequest, $"{nameof(UpdateAreaDto)} is Null");
            }
            DataTier.Entities.Area updateArea = _mapper.Map<DataTier.Entities.Area>(bodyDto);
            updateArea = await _areaRepository.UpdateAreaAsync(updateArea);

             updateAreaDto = _mapper.Map<UpdateAreaDto>(updateArea);
        }
        catch (MException e)
        {
            throw new MException(StatusCodes.Status400BadRequest, e.Message, nameof(e.TargetSite.Name));
        }

        return updateAreaDto;
        
    }

    /// <inheritdoc cref="IAreaService.DeleteAreaAsync"/>
    public async Task<DeleteAreaDto> DeleteAreaAsync(int? id)
    {
        try
        {
            if (id == null)
            {
                throw new MException(StatusCodes.Status400BadRequest, "Id is null");
            }
            
            DataTier.Entities.Area deleteArea = await _areaRepository.DeleteAreaAsync((int)id);
            DeleteAreaDto deleteAreaDto = _mapper.Map<DeleteAreaDto>(deleteArea);
            return deleteAreaDto;
        }
        catch (Exception e)
        {
            throw new MException(StatusCodes.Status400BadRequest, e.Message);
        }
    }
}