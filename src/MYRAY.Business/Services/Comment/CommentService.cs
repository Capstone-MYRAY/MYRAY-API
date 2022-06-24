using AutoMapper;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Comment;
using MYRAY.Business.Enums;
using MYRAY.Business.Helpers;
using MYRAY.Business.Helpers.Paging;
using MYRAY.Business.Repositories.Comment;

namespace MYRAY.Business.Services.Comment;

public class CommentService : ICommentService
{
    private readonly IMapper _mapper;
    private readonly ICommentRepository _commentRepository;

    public CommentService(IMapper mapper, ICommentRepository commentRepository)
    {
        _mapper = mapper;
        _commentRepository = commentRepository;
    }


    public ResponseDto.CollectiveResponse<CommentDetail> GetComments(SearchComment searchComment, PagingDto pagingDto, SortingDto<CommentEnum.CommentSortCriteria> sortingDto,
        int guidePostId)
    {
        IQueryable<DataTier.Entities.Comment> query = _commentRepository.GetCommentByGuidePost(guidePostId);

        query = query.GetWithSearch(searchComment);
        query = query.GetWithSorting(sortingDto.SortColumn.ToString(), sortingDto.OrderBy);

        var result =
            query.GetWithPaging<CommentDetail, DataTier.Entities.Comment>(pagingDto, _mapper);

        return result;
    }

    public async Task<CommentDetail> CreateComment(CreateComment comment, int commentBy)
    {
        DataTier.Entities.Comment dto = _mapper.Map<DataTier.Entities.Comment>(comment);
        dto.CreatedDate = DateTime.Now;
        dto.CommentBy = commentBy;

        dto = await _commentRepository.CreateComment(dto);

        var result = _mapper.Map<CommentDetail>(dto);
        return result;
    }

    public async Task<CommentDetail> UpdateComment(UpdateComment comment, int commentBy)
    {
        
        DataTier.Entities.Comment dto = _mapper.Map<DataTier.Entities.Comment>(comment);
        dto.UpdatedDate = DateTime.Now;
        dto.CommentBy = commentBy;
        dto = await _commentRepository.UpdateComment(dto);

        var result = _mapper.Map<CommentDetail>(dto);
        return result;
    }

    public async Task<CommentDetail> DeleteComment(int id)
    {
        DataTier.Entities.Comment dto = await _commentRepository.DeleteComment(id);
        var result = _mapper.Map<CommentDetail>(dto);
        return result;
    }
}