using Microsoft.EntityFrameworkCore;
using MYRAY.Business.Enums;
using MYRAY.Business.Repositories.Interface;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.Role;

public class RoleRepository : IRoleRepository
{
    private readonly IBaseRepository<DataTier.Entities.Role>? _roleRepo;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleRepository"/> class.
    /// </summary>
    /// <param name="dbContextFactory"></param>
    public RoleRepository(IDbContextFactory dbContextFactory)
    {
        _roleRepo = dbContextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.Role>();
    }

    /// <inheritdoc cref="IRoleRepository.GetRoleByIdAsync"/>
    public async Task<DataTier.Entities.Role> GetRoleByIdAsync(int id)
    {
        DataTier.Entities.Role queryRole = await _roleRepo.GetFirstOrDefaultAsync(r => r.Id == id);
        if (queryRole == null) return null;
        if (queryRole.Status == (int)RoleEnum.RoleStatus.InActive)
        {
            return null;
        }

        return queryRole;
    }

    /// <inheritdoc cref="IRoleRepository.GetRoles"/>
    public IQueryable<DataTier.Entities.Role> GetRoles()
    {
        IQueryable<DataTier.Entities.Role> queryRole = _roleRepo.Get(r =>r.Status == (int)RoleEnum.RoleStatus.Active);
        return queryRole;
    }
}