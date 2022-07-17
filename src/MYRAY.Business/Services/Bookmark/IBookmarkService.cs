using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Bookmark;
using MYRAY.Business.Enums;

namespace MYRAY.Business.Services.Bookmark;

public interface IBookmarkService
{
    /// <summary>
    /// Get list of all bookmark.
    /// </summary>
    /// <returns>list of bookmark.</returns>
    public ResponseDto.CollectiveResponse<BookmarkDetail> GetBookmarks(PagingDto pagingDto,
        SortingDto<BookmarkEnum.BookmarkSortCriteria> sortingDto, int accountId);
    
    /// <summary>
    /// Create an new bookmark.
    /// </summary>
    /// <param name="accountId">Your account id</param>
    /// <param name="bookmarkId">Account Bookmark id</param>
    /// <returns>An bookmark</returns>
    Task<BookmarkDetail> CreateBookmarkAsync(int accountId, int bookmarkId);
    
    /// <summary>
    /// Delete an existed bookmark.
    /// </summary>
    /// <param name="accountId">Your account id</param>
    /// <param name="bookmarkId">Account Bookmark id</param>
    /// <returns>total row affected.</returns>
    Task<BookmarkDetail> DeleteBookmarkAsync(int accountId, int bookmarkId);
}