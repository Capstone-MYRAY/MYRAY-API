using AutoMapper;
using Microsoft.AspNetCore.Http;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.TreeType;
using MYRAY.Business.Enums;
using MYRAY.Business.Exceptions;
using MYRAY.Business.Helpers;
using MYRAY.Business.Helpers.Paging;
using MYRAY.Business.Repositories.TreeType;

namespace MYRAY.Business.Services.TreeType;
/// <summary>
/// Tree type service class.
/// </summary>
public class TreeTypeService : ITreeTypeService
{
    private readonly IMapper _mapper;
    private readonly ITreeTypeRepository _treeTypeRepository;

    /// <summary>
    /// Initialize new instance of the <see cref="TreeTypeService"/> class.
    /// </summary>
    /// <param name="mapper">Injection of <see cref="IMapper"/></param>
    /// <param name="treeTypeRepository">Injection of <see cref="ITreeTypeRepository"/></param>
    public TreeTypeService(IMapper mapper, ITreeTypeRepository treeTypeRepository)
    {
        _mapper = mapper;
        _treeTypeRepository = treeTypeRepository;
    }

    /// <see cref="ITreeTypeService.GetTreeTypes"/>
    public ResponseDto.CollectiveResponse<TreeTypeDetail> GetTreeTypes(SearchTreeType searchTreeType, PagingDto pagingDto, SortingDto<TreeTypeEnum.TreeTypeSortCriteria> sortingDto)
    {
        IQueryable<DataTier.Entities.TreeType> query = _treeTypeRepository.GetTreeTypes();

        query = query.GetWithSearch(searchTreeType);

        query = query.GetWithSorting(sortingDto.SortColumn.ToString(), sortingDto.OrderBy);

        var result = query.GetWithPaging<TreeTypeDetail, DataTier.Entities.TreeType>(pagingDto, _mapper);

        return result;
    }

    /// <see cref="ITreeTypeService.GetTreeTypeById"/>
    public async Task<TreeTypeDetail> GetTreeTypeById(int id)
    {
        if (!int.TryParse(id.ToString(), out _))
        {
            throw new MException(StatusCodes.Status400BadRequest, "Id is number");
        }

        DataTier.Entities.TreeType treeType = await _treeTypeRepository.GetTryTypeByIdAsync(id);
        if (treeType == null)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Tree type is not existed.");
        }

        TreeTypeDetail result = _mapper.Map<TreeTypeDetail>(treeType);

        return result;
    }

    /// <see cref="ITreeTypeService.CreateTreeType"/>
    public async Task<TreeTypeDetail> CreateTreeType(CreateTreeType createTreeType)
    {
        DataTier.Entities.TreeType newTreeType = _mapper.Map<DataTier.Entities.TreeType>(createTreeType);
        newTreeType = await _treeTypeRepository.CreateTreeType(newTreeType);
        var result = _mapper.Map<TreeTypeDetail>(newTreeType);
        return result;
    }

    /// <see cref="ITreeTypeService.UpdateTreeType"/>
    public async Task<TreeTypeDetail> UpdateTreeType(UpdateTreeType updateTreeType)
    {
        DataTier.Entities.TreeType updateTt = _mapper.Map<DataTier.Entities.TreeType>(updateTreeType);
        updateTt = await _treeTypeRepository.UpdateTreeType(updateTt);
        var result = _mapper.Map<TreeTypeDetail>(updateTt);
        return result;
    }
    
    /// <see cref="ITreeTypeService.DeleteTreeType"/>
    public async Task<TreeTypeDetail> DeleteTreeType(int id)
    {
        DataTier.Entities.TreeType treeTypeDb = await  _treeTypeRepository.DeleteTreeType(id);
        var result = _mapper.Map<TreeTypeDetail>(treeTypeDb);
        return result;
    }
}