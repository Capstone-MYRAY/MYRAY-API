using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Garden;
using MYRAY.Business.Enums;

namespace MYRAY.Business.Services.Garden;

public interface IGardenService
{
    ResponseDto.CollectiveResponse<GardenDetail> GetGarden(
        SearchGarden searchGarden,
        PagingDto pagingDto,
        SortingDto<GardenEnum.GardernSortCriteria> sortingDto);
    
    Task<GardenDetail> GetGardenById(int id);
    Task<GardenDetail> CreateGarden(CreateGarden garden);
    Task<GardenDetail> UpdateGarden(UpdateGarden garden);
    Task<GardenDetail> DeleteGarden(int id);

    Task<bool> GetNoAvailableGarden(int gardenId);
}