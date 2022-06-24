using AutoMapper;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Guidepost;
using MYRAY.Business.Enums;
using MYRAY.Business.Helpers;
using MYRAY.Business.Helpers.Paging;
using MYRAY.Business.Repositories.Guidepost;

namespace MYRAY.Business.Services.Guidepost;
/// <summary>
/// Interface for service of Guidepost.
/// </summary>
public class GuidepostService : IGuidepostService
{
    private readonly IMapper _mapper;
    private readonly IGuidepostRepository _guidepostRepository;

    /// <summary>
    /// Initialize new instance of the <see cref="GuidepostService"/> class.
    /// </summary>
    /// <param name="mapper">Injection of <see cref="IMapper"/></param>
    /// <param name="guidepostRepository">Injection of <see cref="IGuidepostRepository"/></param>
    public GuidepostService(IMapper mapper, IGuidepostRepository guidepostRepository)
    {
        _mapper = mapper;
        _guidepostRepository = guidepostRepository;
    }

    /// <see cref="IGuidepostService.GetGuideposts"/>
    public ResponseDto.CollectiveResponse<GuidepostDetail> GetGuideposts(SearchGuidepost searchGuidepost, PagingDto pagingDto, SortingDto<GuidepostEnum.GuidepostSortCriteria> sortingDto)
    {
        IQueryable<DataTier.Entities.Guidepost> query = _guidepostRepository.GetGuideposts();

        query = query.GetWithSearch(searchGuidepost);

        query = query.GetWithSorting(sortingDto.SortColumn.ToString(), sortingDto.OrderBy);

        var result = query.GetWithPaging<GuidepostDetail, DataTier.Entities.Guidepost>(pagingDto, _mapper);

        return result;
    }

    /// <see cref="IGuidepostService.GetGuidepostById"/>
    public async Task<GuidepostDetail> GetGuidepostById(int id)
    {
        
        DataTier.Entities.Guidepost guidepost = await _guidepostRepository.GetGuidepostById(id);
        GuidepostDetail result = _mapper.Map<GuidepostDetail>(guidepost);
        return result;
    }

    /// <see cref="IGuidepostService.CreateGuidepost"/>
    public async Task<GuidepostDetail> CreateGuidepost(CreateGuidepost guidepost, int createBy)
    {
        DataTier.Entities.Guidepost newGuidepost = _mapper.Map<DataTier.Entities.Guidepost>(guidepost);
        newGuidepost.CreatedBy = createBy;
        newGuidepost.CreatedDate = DateTime.Now;
        newGuidepost = await _guidepostRepository.CreateGuidepost(newGuidepost);
        
        var result = _mapper.Map<GuidepostDetail>(newGuidepost);
        return result;
    }

    /// <see cref="IGuidepostService.UpdateGuidepost"/>
    public async Task<GuidepostDetail> UpdateGuidepost(UpdateGuidepost guidepost)
    {
        DataTier.Entities.Guidepost updateGuidepost = _mapper.Map<DataTier.Entities.Guidepost>(guidepost);
        updateGuidepost.UpdatedDate = DateTime.Now;
        updateGuidepost = await _guidepostRepository.UpdateGuidepost(updateGuidepost);
        var result = _mapper.Map<GuidepostDetail>(updateGuidepost);
        return result;
    }

    /// <see cref="IGuidepostService.DeleteGuidepost"/>
    public async Task<GuidepostDetail> DeleteGuidepost(int id)
    {
        DataTier.Entities.Guidepost deleteGuidepost = await _guidepostRepository.DeleteGuidepost(id);
        var result = _mapper.Map<GuidepostDetail>(deleteGuidepost);
        return result;
    }
}