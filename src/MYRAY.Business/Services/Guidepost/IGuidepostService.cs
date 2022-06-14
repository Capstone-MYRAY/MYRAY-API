using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Guidepost;
using MYRAY.Business.Enums;

namespace MYRAY.Business.Services.Guidepost;
/// <summary>
/// Interface for service of Guidepost.
/// </summary>
public interface IGuidepostService
{
    /// <summary>
    /// Get all guidepost with condition
    /// </summary>
    /// <param name="searchGuidepost">Object contains field to filter</param>
    /// <param name="pagingDto">Object contains paging criteria</param>
    /// <param name="sortingDto">Object contains sorting criteria</param>
    /// <returns></returns>
    ResponseDto.CollectiveResponse<GuidepostDetail> GetGuideposts(
        SearchGuidepost searchGuidepost,
        PagingDto pagingDto,
        SortingDto<GuidepostEnum.GuidepostSortCriteria> sortingDto);

    /// <summary>
    /// Get a guidepost by Id
    /// </summary>
    /// <param name="id">Id of guidepost</param>
    /// <returns>A guidepost</returns>
    Task<GuidepostDetail> GetGuidepostById(int id);

    /// <summary>
    /// Create a new guidepost
    /// </summary>
    /// <param name="guidepost">Object contains new guidepost information</param>
    /// <param name="createBy">Id of account create</param>
    /// <returns>A new guidepost.</returns>
    Task<GuidepostDetail> CreateGuidepost(CreateGuidepost guidepost, int createBy);

    /// <summary>
    /// Update an existed guidepost.
    /// </summary>
    /// <param name="guidepost">Object contain update information</param>
    /// <returns>A updated </returns>
    Task<GuidepostDetail> UpdateGuidepost(UpdateGuidepost guidepost);
    
    /// <summary>
    /// Delete an existed guidepost
    /// </summary>
    /// <param name="id">Id of guidepost</param>
    /// <returns>A updated</returns>
    Task<GuidepostDetail> DeleteGuidepost(int id);
}