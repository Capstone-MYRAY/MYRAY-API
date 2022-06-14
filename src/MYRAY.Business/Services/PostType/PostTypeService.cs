using AutoMapper;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.PostType;
using MYRAY.Business.Enums;
using MYRAY.Business.Helpers;
using MYRAY.Business.Helpers.Paging;
using MYRAY.Business.Repositories.PostType;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Services.PostType;

public class PostTypeService : IPostTypeService
{
    private readonly IMapper _mapper;
    private readonly IPostTypeRepository _postTypeRepository;

    public PostTypeService(IMapper mapper, IPostTypeRepository postTypeRepository)
    {
        _mapper = mapper;
        _postTypeRepository = postTypeRepository;
    }

    public ResponseDto.CollectiveResponse<PostTypeDetail> GetPostTypes(SearchPostType searchPostType, PagingDto pagingDto, SortingDto<PostTypeEnum.PostTypeSortCriteria> sortingDto)
    {
        IQueryable<DataTier.Entities.PostType> query = _postTypeRepository.GetPostTypes();

        query = query.GetWithSearch(searchPostType);

        query = query.GetWithSorting(sortingDto.SortColumn.ToString(), sortingDto.OrderBy);

        var result = query.GetWithPaging<PostTypeDetail, DataTier.Entities.PostType>(pagingDto, _mapper);

        return result;
    }

    public async Task<PostTypeDetail> GetPostTypeById(int id)
    {
        DataTier.Entities.PostType postType = await _postTypeRepository.GetPostTypeById(id);
        var result = _mapper.Map<PostTypeDetail>(postType);
        return result;
    }

    public async Task<PostTypeDetail> CreatePostType(CreatePostType postType)
    {
        DataTier.Entities.PostType newPostType = _mapper.Map<DataTier.Entities.PostType>(postType);
        newPostType.Status = (int?)PostTypeEnum.PostTypeStatus.Active;

        newPostType = await _postTypeRepository.CreatePostType(newPostType);

        var result = _mapper.Map<PostTypeDetail>(newPostType);
        return result;
    }

    public async Task<PostTypeDetail> UpdatePostType(UpdatePostType postType)
    {
        DataTier.Entities.PostType updatePostType = _mapper.Map<DataTier.Entities.PostType>(postType);
        updatePostType.Status = (int?)PostTypeEnum.PostTypeStatus.Active;

        updatePostType = await _postTypeRepository.UpdatePostType(updatePostType);

        var result = _mapper.Map<PostTypeDetail>(updatePostType);
        return result;
        
    }

    public async Task<PostTypeDetail> DeletePostType(int id)
    {
        DataTier.Entities.PostType postType = await _postTypeRepository.DeletePostType(id);
        var result = _mapper.Map<PostTypeDetail>(postType);
        return result;
    }
}