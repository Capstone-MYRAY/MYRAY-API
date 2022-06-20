using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Feedback;
using MYRAY.Business.Enums;

namespace MYRAY.Business.Services.Feedback;

public interface IFeedbackService
{
    ResponseDto.CollectiveResponse<FeedbackDetail> GetFeedbacks(
        SearchFeedback searchFeedback,
        PagingDto pagingDto,
        SortingDto<FeedbackEnum.FeedbackSortCriteria> sortingDto);
    
    Task<FeedbackDetail> GetFeedbackById(int id);
    Task<FeedbackDetail> CreateFeedback(CreateFeedback feedback, int createBy);
    Task<FeedbackDetail> UpdateFeedback(UpdateFeedback feedback, int createBy);
    Task<FeedbackDetail> DeleteFeedback(int id);
}