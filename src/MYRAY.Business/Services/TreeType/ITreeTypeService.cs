using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.TreeType;
using MYRAY.Business.Enums;

namespace MYRAY.Business.Services.TreeType;
/// <summary>
/// Interface for tree type service
/// </summary>
public interface ITreeTypeService
{
    /// <summary>
    /// Get list of tree type with condition.
    /// </summary>
    /// <param name="searchTreeType">Object contains field to filter.</param>
    /// <param name="pagingDto">Object contains paging criteria.</param>
    /// <param name="sortingDto">Object contains sorting criteria.</param>
    /// <returns></returns>
    public ResponseDto.CollectiveResponse<TreeTypeDetail> GetTreeTypes(SearchTreeType searchTreeType
        , PagingDto pagingDto, SortingDto<TreeTypeEnum.TreeTypeSortCriteria> sortingDto);

    /// <summary>
    /// Get a tree type by Id
    /// </summary>
    /// <param name="id">Id of tree type</param>
    /// <returns>A tree type</returns>
    public Task<TreeTypeDetail> GetTreeTypeById(int id);

    /// <summary>
    /// Create a new tree type
    /// </summary>
    /// <param name="createTreeType">Object contains information of new tree type </param>
    /// <returns>A new tree type</returns>
    public Task<TreeTypeDetail> CreateTreeType(CreateTreeType createTreeType);

    /// <summary>
    /// Update an existed tree type.
    /// </summary>
    /// <param name="updateTreeType">Object contains update information</param>
    /// <returns>A tree type.</returns>
    public Task<TreeTypeDetail> UpdateTreeType(UpdateTreeType updateTreeType);

    /// <summary>
    /// Delete an existed tree type
    /// </summary>
    /// <param name="id">Id of tree type</param>
    /// <returns>A deleted tree type.</returns>
    public Task<TreeTypeDetail> DeleteTreeType(int id);
}