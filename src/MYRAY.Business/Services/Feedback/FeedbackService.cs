using AutoMapper;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Feedback;
using MYRAY.Business.Enums;
using MYRAY.Business.Helpers;
using MYRAY.Business.Helpers.Paging;
using MYRAY.Business.Repositories.Feedback;

namespace MYRAY.Business.Services.Feedback;

public class FeedbackService : IFeedbackService
{
    private readonly IMapper _mapper;
    private readonly IFeedbackRepository _feedbackRepository;

    public FeedbackService(IMapper mapper, IFeedbackRepository feedbackRepository)
    {
        _mapper = mapper;
        _feedbackRepository = feedbackRepository;
    }

    public ResponseDto.CollectiveResponse<FeedbackDetail> GetFeedbacks(SearchFeedback searchFeedback, PagingDto pagingDto, SortingDto<FeedbackEnum.FeedbackSortCriteria> sortingDto)
    {
        IQueryable<DataTier.Entities.Feedback> query = _feedbackRepository.GetFeedbacks();

        query = query.GetWithSearch(searchFeedback);

        query = query.GetWithSorting(sortingDto.SortColumn.ToString(), sortingDto.OrderBy);

        var result = query.GetWithPaging<FeedbackDetail, DataTier.Entities.Feedback>(pagingDto, _mapper);

        return result;
    }

    public async Task<FeedbackDetail> GetFeedbackById(int id)
    {
        DataTier.Entities.Feedback report = await _feedbackRepository.GetFeedbackById(id);
        FeedbackDetail result = _mapper.Map<FeedbackDetail>(report);
        return result;
    }

    public async Task<FeedbackDetail> CreateFeedback(CreateFeedback feedback, int createBy)
    {
        DataTier.Entities.Feedback feedbackOri = _mapper.Map<DataTier.Entities.Feedback>(feedback);
        feedbackOri.CreatedBy = createBy;
        feedbackOri.CreatedDate = DateTime.Now;
        feedbackOri = await _feedbackRepository.CreateFeedback(feedbackOri);
        FeedbackDetail result = _mapper.Map<FeedbackDetail>(feedbackOri);
        return result;
    }

    public async Task<FeedbackDetail> UpdateFeedback(UpdateFeedback feedback, int createBy)
    {
        DataTier.Entities.Feedback feedbackOri = _mapper.Map<DataTier.Entities.Feedback>(feedback);
        feedbackOri.CreatedBy = createBy;
        feedbackOri.CreatedDate = DateTime.Now;
        feedbackOri = await _feedbackRepository.UpdateFeedback(feedbackOri);
        FeedbackDetail result = _mapper.Map<FeedbackDetail>(feedbackOri);
        return result;
    }

    public async Task<FeedbackDetail> DeleteFeedback(int id)
    {
        DataTier.Entities.Feedback report = await _feedbackRepository.DeleteFeedback(id);
        FeedbackDetail result = _mapper.Map<FeedbackDetail>(report);
        return result;
    }
}