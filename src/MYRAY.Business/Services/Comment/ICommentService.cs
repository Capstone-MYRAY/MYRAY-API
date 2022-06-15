using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Comment;
using MYRAY.Business.Enums;

namespace MYRAY.Business.Services.Comment;

public interface ICommentService
{
    ResponseDto.CollectiveResponse<CommentDetail> GetComments(
        SearchComment searchComment,
        PagingDto pagingDto,
        SortingDto<CommentEnum.CommentSortCriteria> sortingDto,
        int guidePostId);

    Task<CommentDetail> CreateComment(CreateComment comment, int commentBy);
    Task<CommentDetail> UpdateComment(UpdateComment comment, int commentBy);
    Task<CommentDetail> DeleteComment(int id);
}