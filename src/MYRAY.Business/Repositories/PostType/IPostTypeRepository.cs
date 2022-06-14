namespace MYRAY.Business.Repositories.PostType;

public interface IPostTypeRepository
{
    IQueryable<DataTier.Entities.PostType> GetPostTypes();
    Task<DataTier.Entities.PostType> GetPostTypeById(int id);
    Task<DataTier.Entities.PostType> CreatePostType(DataTier.Entities.PostType postType);
    Task<DataTier.Entities.PostType> UpdatePostType(DataTier.Entities.PostType postType);
    Task<DataTier.Entities.PostType> DeletePostType(int id);
}