namespace MYRAY.Business.Repositories.Area;

/// <summary>
/// Interface for Area repository.
/// </summary>
public interface IAreaRepository
{
    /// <summary>
    /// Get area by area's ID
    /// </summary>
    /// <param name="id">ID of area.</param>
    /// <returns>An Area or null if not existed.</returns>
    Task<DataTier.Entities.Area?> GetAreaByIdAsync(int id);

    /// <summary>
    /// Get all area.
    /// </summary>
    /// <returns>list of area.</returns>
    IQueryable<DataTier.Entities.Area> GetAreas();

    /// <summary>
    /// Create a new area.
    /// </summary>
    /// <param name="area">Area Object to insert</param>
    /// <returns>A new area.</returns>
    Task<DataTier.Entities.Area> CreateAreaAsync(DataTier.Entities.Area area);

    /// <summary>
    /// Update an exist area.
    /// </summary>
    /// <param name="area">Area Object to update</param>
    /// <returns>A new updated area.</returns>
    Task<DataTier.Entities.Area> UpdateAreaAsync(DataTier.Entities.Area area);

    /// <summary>
    /// Delete an area by change status to Inactive
    /// </summary>
    /// <param name="area">Area object to delete</param>
    /// <returns>object affected.</returns>
    Task<DataTier.Entities.Area> DeleteAreaAsync(DataTier.Entities.Area area);

    /// <summary>
    /// Delete a area by ID
    /// </summary>
    /// <param name="id">ID of area</param>
    /// <returns>object affected.</returns>
    Task<DataTier.Entities.Area> DeleteAreaAsync(int id);


}