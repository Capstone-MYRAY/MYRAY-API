using AutoMapper;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Bookmark;
using MYRAY.Business.Enums;
using MYRAY.Business.Helpers.Paging;
using MYRAY.Business.Repositories.Bookmark;
using MYRAY.Business.Repositories.Interface;

namespace MYRAY.Business.Services.Bookmark;

public class BookmarkService : IBookmarkService
{
    private readonly IMapper _mapper;
    private readonly IBookmarkRepository _bookmarkRepository;

    public BookmarkService(IMapper mapper, IBookmarkRepository bookmarkRepository)
    {
        _mapper = mapper;
        _bookmarkRepository = bookmarkRepository;
    }


    public ResponseDto.CollectiveResponse<BookmarkDetail> GetBookmarks(PagingDto pagingDto,
        SortingDto<BookmarkEnum.BookmarkSortCriteria> sortingDto, int accountId)
    {
        IQueryable<DataTier.Entities.Bookmark> queryable = _bookmarkRepository.GetBookmarks(accountId);

        queryable = queryable.GetWithSorting(sortingDto.SortColumn.ToString(), sortingDto.OrderBy);

        var result = queryable.GetWithPaging<BookmarkDetail, DataTier.Entities.Bookmark>(pagingDto, _mapper);
        return result;
    }

    public async Task<BookmarkDetail?> GetBookmarkByIdAsync(int accountId, int bookmarkId)
    {
        DataTier.Entities.Bookmark? bookmark = await _bookmarkRepository.GetBookmarksById(accountId, bookmarkId);
        if (bookmark == null) 
            return null;
        BookmarkDetail result = _mapper.Map<BookmarkDetail>(bookmark);
        return result;
    }

    public async Task<BookmarkDetail> CreateBookmarkAsync(int accountId, int bookmarkId)
    {
        DataTier.Entities.Bookmark bookmark = await _bookmarkRepository.CreateBookmark(accountId, bookmarkId);
        BookmarkDetail result = _mapper.Map<BookmarkDetail>(bookmark);
        return result;
    }

    public async Task<BookmarkDetail> DeleteBookmarkAsync(int accountId, int bookmarkId)
    {
        DataTier.Entities.Bookmark bookmark = await _bookmarkRepository.DeleteBookmark(accountId, bookmarkId);
        BookmarkDetail result = _mapper.Map<BookmarkDetail>(bookmark);
        return result;
    }
}