namespace MYRAY.Business.Repositories.Guidepost;
/// <summary>
/// Interface for Guidepost repository
/// </summary>
public interface IGuidepostRepository
{
    /// <summary>
    /// Get all guidepost
    /// </summary>
    /// <returns>List of guidepost</returns>
    IQueryable<DataTier.Entities.Guidepost> GetGuideposts();

    /// <summary>
    /// Get a guidepost by Id
    /// </summary>
    /// <returns>A guidepost</returns>
    Task<DataTier.Entities.Guidepost> GetGuidepostById(int id);

    /// <summary>
    /// Create a new guidepost
    /// </summary>
    /// <param name="guidepost">Guidepost entity</param>
    /// <returns>A new guidepost</returns>
    Task<DataTier.Entities.Guidepost> CreateGuidepost(DataTier.Entities.Guidepost guidepost);

    /// <summary>
    /// Update an existed guidepost
    /// </summary>
    /// <param name="guidepost">Guidepost entity</param>
    /// <returns>a updated guidepost</returns>
    Task<DataTier.Entities.Guidepost> UpdateGuidepost(DataTier.Entities.Guidepost guidepost);

    /// <summary>
    /// Delete an existed guidepost
    /// </summary>
    /// <param name="id">Id of guidepost</param>
    /// <returns>An deleted guidepost</returns>
    Task<DataTier.Entities.Guidepost> DeleteGuidepost(int id);
}