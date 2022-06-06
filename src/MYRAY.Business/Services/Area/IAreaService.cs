using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Area;
using MYRAY.Business.DTOs.Role;
using MYRAY.Business.Enums;

namespace MYRAY.Business.Services.Area;
/// <summary>
/// Interface for service of Area.
/// </summary>
public interface IAreaService
{
    /// <summary>
    /// Get list of all area.
    /// </summary>
    /// <returns>list of area.</returns>
    public ResponseDto.CollectiveResponse<GetAreaDetail> GetAreas(PagingDto pagingDto,
        SortingDto<AreaEnum.AreaSortCriteria> sortingDto, SearchAreaDto searchAreaDto);
    
    /// <summary>
    /// Get detail information of an Area..
    /// </summary>
    /// <param name="id">ID of area.</param>
    /// <returns>An area or null if not found</returns>
    Task<GetAreaDetail> GetAreaByIdAsync(int? id);

    /// <summary>
    /// Create an new area.
    /// </summary>
    /// <param name="bodyDto">An object contains info to insert area.</param>
    /// <returns>An area</returns>
    Task<GetAreaDetail> CreateAreaAsync(InsertAreaDto? bodyDto);
    
    /// <summary>
    /// Update an existed area.
    /// </summary>
    /// <param name="bodyDto">An object contains info to update area.</param>
    /// <returns>An area.</returns>
    Task<UpdateAreaDto> UpdateAreaAsync(UpdateAreaDto bodyDto);

    /// <summary>
    /// Delete an existed area.
    /// </summary>
    /// <param name="id">ID of area.</param>
    /// <returns>total row affected.</returns>
    Task<DeleteAreaDto> DeleteAreaAsync(int? id);
}