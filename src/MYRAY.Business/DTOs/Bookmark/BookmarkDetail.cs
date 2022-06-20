namespace MYRAY.Business.DTOs.Bookmark;

public class BookmarkDetail
{
    public int AccountId { get; set; }
    public int BookmarkId { get; set; }
    public virtual DataTier.Entities.Account BookmarkNavigation { get; set; } = null!;
}