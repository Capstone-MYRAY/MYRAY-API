using MYRAY.Business.Repositories.Interface;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.Bookmark;

public class BookmarkRepository : IBookmarkRepository
{
    private readonly IDbContextFactory _contextFactory;
    private readonly IBaseRepository<DataTier.Entities.Bookmark> _bookmarkRepository;

    public BookmarkRepository(IDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
        _bookmarkRepository = _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.Bookmark>()!;
    }

    public IQueryable<DataTier.Entities.Bookmark> GetBookmarks(int accountId)
    {
        IQueryable<DataTier.Entities.Bookmark> query = _bookmarkRepository.Get(b => b.AccountId == accountId);
        return query;
    }

    public async Task<DataTier.Entities.Bookmark> CreateBookmark(int accountId, int bookmarkId)
    {
        try
        {
            DataTier.Entities.Bookmark bookmark = new DataTier.Entities.Bookmark()
            {
                AccountId = accountId,
                BookmarkId = bookmarkId,
                CreatedDate = DateTime.Now
            };
            await _bookmarkRepository.InsertAsync(bookmark);

            await _contextFactory.SaveAllAsync();
            return bookmark;
        }
        catch (Exception e)
        {
            throw new Exception("Id has been marked");
        }
    }

    public async Task<DataTier.Entities.Bookmark> DeleteBookmark(int accountId, int bookmarkId)
    {
        DataTier.Entities.Bookmark bookmark = (await _bookmarkRepository.GetFirstOrDefaultAsync(bookmark =>
            bookmark.AccountId == accountId && bookmark.BookmarkId == bookmarkId))!;

        if (bookmark == null)
        {
            throw new Exception("NO Bookmark");
        }
        
        _bookmarkRepository.Delete(bookmark);

        await _contextFactory.SaveAllAsync();
        return bookmark;
    }
}