namespace MYRAY.Business.Enums;

public class CommentEnum
{
    public enum CommentStatus
    {
        Active = 1,
        Deleted = 0
    }

    public enum CommentSortCriteria
    {
        CommentBy,
        CreateDate
    }
}