using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.PostType;
using MYRAY.Business.Enums;

namespace MYRAY.Business.Services.PostType;

public interface IPostTypeService
{
    ResponseDto.CollectiveResponse<PostTypeDetail> GetPostTypes(
        SearchPostType searchPostType,
        PagingDto pagingDto,
        SortingDto<PostTypeEnum.PostTypeSortCriteria> sortingDto);
    
    Task<PostTypeDetail> GetPostTypeById(int id);
    Task<PostTypeDetail> CreatePostType(CreatePostType postType);
    Task<PostTypeDetail> UpdatePostType(UpdatePostType postType);
    Task<PostTypeDetail> DeletePostType(int id);
}