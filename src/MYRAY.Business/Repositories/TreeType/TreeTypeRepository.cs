using Microsoft.AspNetCore.Http;
using MYRAY.Business.Enums;
using MYRAY.Business.Exceptions;
using MYRAY.Business.Repositories.Interface;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.TreeType;
/// <inheritdoc cref="ITreeTypeRepository"/>
public class TreeTypeRepository : ITreeTypeRepository
{
    private readonly IDbContextFactory _contextFactory;
    private readonly IBaseRepository<DataTier.Entities.TreeType> _treeTypeRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="TreeTypeRepository"/> class.
    /// </summary>
    /// <param name="contextFactory">Injection of <see cref="IDbContextFactory"/></param>
    public TreeTypeRepository(IDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
        _treeTypeRepository = _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.TreeType>()!;
    }
    
    /// <see cref="ITreeTypeRepository.GetTreeTypes"/>
    public IQueryable<DataTier.Entities.TreeType> GetTreeTypes()
    {
        IQueryable<DataTier.Entities.TreeType> query =
            _treeTypeRepository.Get();
        return query;
    }
    
    /// <see cref="ITreeTypeRepository.GetTryTypeByIdAsync"/>
    public async Task<DataTier.Entities.TreeType> GetTryTypeByIdAsync(int id)
    {
        DataTier.Entities.TreeType treeType =
            (await _treeTypeRepository.GetFirstOrDefaultAsync(tp =>
                tp.Id == id && tp.Status == (int)TreeTypeEnum.TreeTypeStatus.Active))!;
        return treeType;
    }

    /// <see cref="ITreeTypeRepository.CreateTreeType"/>
    public async Task<DataTier.Entities.TreeType> CreateTreeType(DataTier.Entities.TreeType treeType)
    {
        await _treeTypeRepository.InsertAsync(treeType);

        await _contextFactory.SaveAllAsync();

        return treeType;
    }

    /// <see cref="ITreeTypeRepository.UpdateTreeType"/>
    public async Task<DataTier.Entities.TreeType> UpdateTreeType(DataTier.Entities.TreeType treeType)
    {
        _treeTypeRepository.Modify(treeType);

        await _contextFactory.SaveAllAsync();

        return treeType;
    }

    /// <see cref="ITreeTypeRepository.DeleteTreeType"/>
    public async Task<DataTier.Entities.TreeType> DeleteTreeType(int id)
    {
        DataTier.Entities.TreeType? treeType = await _treeTypeRepository.GetByIdAsync(id);
        if (treeType == null)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Tree type is not found.");
        }

        treeType.Status = (int)TreeTypeEnum.TreeTypeStatus.Inactive;
        
        _treeTypeRepository.Modify(treeType);
        
        await _contextFactory.SaveAllAsync();

        return treeType;
    }
}