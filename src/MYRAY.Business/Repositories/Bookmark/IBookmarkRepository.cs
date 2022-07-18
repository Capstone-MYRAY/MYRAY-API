namespace MYRAY.Business.Repositories.Bookmark;

public interface IBookmarkRepository
{
    IQueryable<DataTier.Entities.Bookmark> GetBookmarks(int accountId);
    Task<MYRAY.DataTier.Entities.Bookmark?> GetBookmarksById(int accountId, int bookmarkId);
    Task<DataTier.Entities.Bookmark> CreateBookmark(int accountId, int bookmarkId);
    Task<DataTier.Entities.Bookmark> DeleteBookmark(int accountId,int bookmarkId);
}