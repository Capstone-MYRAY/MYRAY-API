namespace MYRAY.Business.Repositories.TreeType;
/// <summary>
/// Interface for Tree Type repository
/// </summary>
public interface ITreeTypeRepository
{
    /// <summary>
    /// Get all tree type
    /// </summary>
    /// <returns></returns>
    IQueryable<MYRAY.DataTier.Entities.TreeType> GetTreeTypes();
    
    /// <summary>
    /// Get a tree type by ID
    /// </summary>
    /// <param name="id">Id of tree type</param>
    /// <returns></returns>
    Task<DataTier.Entities.TreeType> GetTryTypeByIdAsync(int id);
    
    /// <summary>
    /// Create new tree type
    /// </summary>
    /// <param name="treeType">Object contains new tree type information</param>
    /// <returns>A new tree type</returns>
    Task<DataTier.Entities.TreeType> CreateTreeType(DataTier.Entities.TreeType treeType);
    
    /// <summary>
    /// Update an existed tree type
    /// </summary>
    /// <param name="treeType">Object contains updated tree type information</param>
    /// <returns>A updated tree type</returns>
    Task<DataTier.Entities.TreeType> UpdateTreeType(DataTier.Entities.TreeType treeType);
    
    /// <summary>
    /// Delete an existed tree type
    /// </summary>
    /// <param name="id">Id of tree type</param>
    /// <returns>a tree type</returns>
    Task<DataTier.Entities.TreeType> DeleteTreeType(int id);

}