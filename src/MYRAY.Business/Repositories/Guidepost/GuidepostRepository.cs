using Microsoft.AspNetCore.Http;
using MYRAY.Business.Enums;
using MYRAY.Business.Exceptions;
using MYRAY.Business.Repositories.Interface;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.Guidepost;
/// <inheritdoc cref="IGuidepostRepository"/>
public class GuidepostRepository : IGuidepostRepository
{
    private readonly IDbContextFactory _contextFactory;
    private readonly IBaseRepository<DataTier.Entities.Guidepost> _guidepostRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GuidepostRepository"/> class.
    /// </summary>
    /// <param name="contextFactory">Injection of <see cref="IDbContextFactory"/></param>
    public GuidepostRepository(IDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
        _guidepostRepository = _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.Guidepost>()!;
    }

    /// <inheritdoc cref="IGuidepostRepository.GetGuideposts"/>
    public IQueryable<DataTier.Entities.Guidepost> GetGuideposts()
    {
        IQueryable<DataTier.Entities.Guidepost> query =
            _guidepostRepository.Get(g => g.Status == (int?)GuidepostEnum.GuidepostStatus.Active);
        return query;
    }

    /// <inheritdoc cref="IGuidepostRepository.GetGuidepostById"/>
    public async Task<DataTier.Entities.Guidepost> GetGuidepostById(int id)
    {
        DataTier.Entities.Guidepost guidepost = (await _guidepostRepository
            .GetFirstOrDefaultAsync(g => g.Id == id && g.Status == (int?)GuidepostEnum.GuidepostStatus.Active)!)!;
        return guidepost;
    }

    /// <inheritdoc cref="IGuidepostRepository.CreateGuidepost"/>
    public async Task<DataTier.Entities.Guidepost> CreateGuidepost(DataTier.Entities.Guidepost guidepost)
    {
        await _guidepostRepository.InsertAsync(guidepost);

        await _contextFactory.SaveAllAsync();

        return guidepost;
    }

    /// <inheritdoc cref="IGuidepostRepository.UpdateGuidepost"/>
    public async Task<DataTier.Entities.Guidepost> UpdateGuidepost(DataTier.Entities.Guidepost guidepost)
    {
        _guidepostRepository.Modify(guidepost);
        
        await _contextFactory.SaveAllAsync();

        return guidepost;
    }

    /// <inheritdoc cref="IGuidepostRepository.DeleteGuidepost"/>
    public async Task<DataTier.Entities.Guidepost> DeleteGuidepost(int id)
    {
        DataTier.Entities.Guidepost? guidepost = await _guidepostRepository.GetByIdAsync(id);

        if (guidepost == null)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Guidepost is not existed.");
        }

        guidepost.Status = (int?)GuidepostEnum.GuidepostStatus.Inactive;
        
        _guidepostRepository.Modify(guidepost);

        await _contextFactory.SaveAllAsync();

        return guidepost;
    }
}