using AutoMapper;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Garden;
using MYRAY.Business.Enums;
using MYRAY.Business.Helpers;
using MYRAY.Business.Helpers.Paging;
using MYRAY.Business.Repositories.Garden;

namespace MYRAY.Business.Services.Garden;

public class GardenService : IGardenService
{
    private readonly IMapper _mapper;
    private readonly IGardenRepository _gardenRepository;

    public GardenService(IMapper mapper, IGardenRepository gardenRepository)
    {
        _mapper = mapper;
        _gardenRepository = gardenRepository;
    }


    public ResponseDto.CollectiveResponse<GardenDetail> GetGarden(SearchGarden searchGarden, PagingDto pagingDto, SortingDto<GardenEnum.GardernSortCriteria> sortingDto)
    {
        IQueryable<DataTier.Entities.Garden> query = _gardenRepository.GetGardens();

        query = query.GetWithSearch(searchGarden);

        query = query.GetWithSorting(sortingDto.SortColumn.ToString(), sortingDto.OrderBy);

        var result = query.GetWithPaging<GardenDetail, DataTier.Entities.Garden>(pagingDto, _mapper);

        return result;
    }

    public async Task<GardenDetail> GetGardenById(int id)
    {
        DataTier.Entities.Garden garden = await _gardenRepository.GetGardenById(id);
        GardenDetail result = _mapper.Map<GardenDetail>(garden);
        return result;
    }

    public async Task<GardenDetail> CreateGarden(CreateGarden garden)
    {
        DataTier.Entities.Garden gardenDto = _mapper.Map<DataTier.Entities.Garden>(garden);
        gardenDto.CreatedDate = DateTime.Now;
        gardenDto.Status = (int?)GardenEnum.GardenStatus.Active;
        gardenDto = await _gardenRepository.CreateGarden(gardenDto);
        GardenDetail result = _mapper.Map<GardenDetail>(gardenDto);
        return result;
    }

    public async Task<GardenDetail> UpdateGarden(UpdateGarden garden)
    {
        DataTier.Entities.Garden gardenDto = _mapper.Map<DataTier.Entities.Garden>(garden);
        gardenDto.UpdatedDate = DateTime.Now;
        gardenDto = await _gardenRepository.UpdateGarden(gardenDto);
        GardenDetail result = _mapper.Map<GardenDetail>(gardenDto);

        return result;
    }

    public async Task<GardenDetail> DeleteGarden(int id)
    {
        DataTier.Entities.Garden garden = await _gardenRepository.DeleteGarden(id);
        GardenDetail result = _mapper.Map<GardenDetail>(garden);
        return result;
    }
}