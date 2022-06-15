namespace MYRAY.Business.Repositories.Comment;

public interface ICommentRepository
{
    IQueryable<DataTier.Entities.Comment> GetCommentByGuidePost(int id);
    Task<DataTier.Entities.Comment> CreateComment(DataTier.Entities.Comment comment);
    Task<DataTier.Entities.Comment> UpdateComment(DataTier.Entities.Comment comment);
    Task<DataTier.Entities.Comment> DeleteComment(int id);
}