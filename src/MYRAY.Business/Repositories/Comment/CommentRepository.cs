using System.Linq.Expressions;
using MYRAY.Business.Enums;
using MYRAY.Business.Repositories.Interface;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.Comment;

public class CommentRepository : ICommentRepository
{
    private readonly IDbContextFactory _contextFactory;
    private readonly IBaseRepository<DataTier.Entities.Comment> _commentRepository;

    public CommentRepository(IDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
        _commentRepository = _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.Comment>()!;
    }


    public IQueryable<DataTier.Entities.Comment> GetCommentByGuidePost(int id)
    {
        Expression<Func<DataTier.Entities.Comment, object>> expAccount = comment => comment.CommentByNavigation;
        IQueryable<DataTier.Entities.Comment> query =
            _commentRepository.Get(c => c.GuidepostId == id && c.Status 
                == (int?)CommentEnum.CommentStatus.Active, new []{expAccount});
        return query;
    }

    public async Task<DataTier.Entities.Comment> CreateComment(DataTier.Entities.Comment comment)
    {
        await _commentRepository.InsertAsync(comment);

        await _contextFactory.SaveAllAsync();

        return comment;
    }

    public async Task<DataTier.Entities.Comment> UpdateComment(DataTier.Entities.Comment comment)
    {
        DataTier.Entities.Comment cmDb = _commentRepository.GetById(comment.Id);
        comment.GuidepostId = cmDb.GuidepostId;
        comment.CreatedDate = cmDb.CreatedDate;
        _commentRepository.Modify(comment);
        await _contextFactory.SaveAllAsync();

        return comment;
    }

    public async Task<DataTier.Entities.Comment> DeleteComment(int id)
    {
        DataTier.Entities.Comment comment = _commentRepository.GetById(id)!;
        if (comment == null || comment.Status == (int?)CommentEnum.CommentStatus.Deleted)
        {
            throw new Exception("Comment is not existed");
        }

        comment.Status = (int?)CommentEnum.CommentStatus.Deleted;
        
        _commentRepository.Modify(comment);
        await _contextFactory.SaveAllAsync();

        return comment;
    }
}