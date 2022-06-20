using MYRAY.Business.Repositories.Interface;

namespace MYRAY.Business.Repositories.Bookmark;

public class BookmarkRepository : IBookmarkRepository
{
    private readonly IDbContextFactory _contextFactory;
    private readonly IBaseRepository<DataTier.Entities.Bookmark> _bookmarkRepository;

    public BookmarkRepository(IDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public IQueryable<DataTier.Entities.Bookmark> GetBookmarks(int accountId)
    {
        return null;
    }

    public Task<DataTier.Entities.Bookmark> CreateBookmark(int bookmarkId)
    {
        return null;
    }

    public Task<DataTier.Entities.Bookmark> DeleteBookmark(int id)
    {
        return null;
    }
}