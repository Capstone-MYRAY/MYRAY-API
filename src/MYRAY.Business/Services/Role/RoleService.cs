using AutoMapper;
using Microsoft.AspNetCore.Http;
using MYRAY.Business.DTOs.Role;
using MYRAY.Business.Exceptions;
using MYRAY.Business.Repositories.Role;

namespace MYRAY.Business.Services.Role;
/// <summary>
/// Service of Role.
/// </summary>
public class RoleService : IRoleService
{
    private readonly IMapper _mapper;
    private IRoleRepository _roleRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleService"/> class.
    /// </summary>
    /// <param name="mapper">Injection <see cref="IMapper"/></param>
    /// <param name="roleRepository">Injection <see cref="IRoleRepository"/></param>
    public RoleService(IMapper mapper, IRoleRepository roleRepository)
    {
        _mapper = mapper;
        _roleRepository = roleRepository;
    }
    
    /// <inheritdoc cref="IRoleService.GetRoleList"/>
    public IEnumerable<GetRoleDetail> GetRoleList()
    {
        IQueryable<DataTier.Entities.Role> queryRole = _roleRepository.GetRoles();

        IEnumerable<GetRoleDetail> queryRoleDto = _mapper.ProjectTo<GetRoleDetail>(queryRole);

        return queryRoleDto;
    }

    /// <inheritdoc cref="IRoleService.GetRoleById"/>
    public async Task<GetRoleDetail> GetRoleById(int? id)
    {
        if (id == null)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Input Id to get");
        }
        int idRole;
        if (!int.TryParse(id.ToString(), out idRole))
        {
            throw new MException(StatusCodes.Status400BadRequest, "ID must be Number");
        }

        DataTier.Entities.Role queryRole = await _roleRepository.GetRoleByIdAsync(idRole);

        GetRoleDetail roleDto = _mapper.Map<GetRoleDetail>(queryRole);

        return roleDto;
    }
}