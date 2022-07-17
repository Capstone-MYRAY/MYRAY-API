using MYRAY.Business.DTOs.Account;

namespace MYRAY.Business.DTOs.Bookmark;

public class BookmarkDetail
{
    public int AccountId { get; set; }
    public int BookmarkId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public virtual GetAccountDetail BookmarkNavigation { get; set; } = null!;
}