using Microsoft.AspNetCore.Http;
using MYRAY.Business.Enums;
using MYRAY.Business.Exceptions;
using MYRAY.Business.Repositories.Interface;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.PostType;

public class PostTypeRepository : IPostTypeRepository
{
    private readonly IDbContextFactory _contextFactory;
    private readonly IBaseRepository<DataTier.Entities.PostType> _postTypeRepository;

    public PostTypeRepository(IDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
        _postTypeRepository = _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.PostType>()!;
    }


    public IQueryable<DataTier.Entities.PostType> GetPostTypes()
    {
        IQueryable<DataTier.Entities.PostType> query = _postTypeRepository.Get(pt => pt.Status != (int?)PostTypeEnum.PostTypeStatus.Inactive);
        return query;
    }

    public async Task<DataTier.Entities.PostType> GetPostTypeById(int id)
    {
        DataTier.Entities.PostType postType = (await 
            _postTypeRepository.GetFirstOrDefaultAsync(pt =>
                pt.Status != (int?)PostTypeEnum.PostTypeStatus.Inactive && pt.Id == id))!;
        return postType;
    }

    public async Task<DataTier.Entities.PostType> CreatePostType(DataTier.Entities.PostType postType)
    {
        await _postTypeRepository.InsertAsync(postType);

        await _contextFactory.SaveAllAsync();

        return postType;
    }

    public async Task<DataTier.Entities.PostType> UpdatePostType(DataTier.Entities.PostType postType)
    {
        _postTypeRepository.Modify(postType);

        await _contextFactory.SaveAllAsync();

        return postType;
    }

    public async Task<DataTier.Entities.PostType> DeletePostType(int id)
    {
        DataTier.Entities.PostType? postType = await _postTypeRepository.GetByIdAsync(id);

        if (postType == null)
        {
            throw new MException(StatusCodes.Status400BadRequest, "PostType is not existed.");
        }

        postType.Status = (int?)PostTypeEnum.PostTypeStatus.Inactive;
        
        _postTypeRepository.Modify(postType);

        await _contextFactory.SaveAllAsync();

        return postType;
    }
}