using MYRAY.Business.Repositories.Interface;

namespace MYRAY.Business.Repositories.Role;
/// <summary>
/// Interface for repository Role entity.
/// </summary>
public interface IRoleRepository
{
    /// <summary>
    /// Get Role by ID.
    /// </summary>
    /// <param name="id">ID of Role.</param>
    /// <returns>a role record.</returns>
    Task<DataTier.Entities.Role> GetRoleByIdAsync(int id);
    
    /// <summary>
    /// Get all roles.
    /// </summary>
    /// <returns>list of all roles</returns>
    IQueryable<DataTier.Entities.Role> GetRoles();
}