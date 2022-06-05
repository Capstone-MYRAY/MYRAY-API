using MYRAY.Business.DTOs.Role;

namespace MYRAY.Business.Services.Role;
/// <summary>
/// Interface for service of Role.
/// </summary>
public interface IRoleService
{
    /// <summary>
    /// Get list of all role.
    /// </summary>
    /// <returns></returns>
    IEnumerable<GetRoleDetail> GetRoleList();
    
    /// <summary>
    /// Get detail information of a Role.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<GetRoleDetail> GetRoleById(int? id);
}