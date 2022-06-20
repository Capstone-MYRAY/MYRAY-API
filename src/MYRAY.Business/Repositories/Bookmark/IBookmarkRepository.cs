namespace MYRAY.Business.Repositories.Bookmark;

public interface IBookmarkRepository
{
    IQueryable<DataTier.Entities.Bookmark> GetBookmarks(int accountId);
    Task<DataTier.Entities.Bookmark> CreateBookmark(int bookmarkId);
    Task<DataTier.Entities.Bookmark> DeleteBookmark(int id);
}