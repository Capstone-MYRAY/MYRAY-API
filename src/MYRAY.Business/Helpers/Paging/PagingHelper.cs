using AutoMapper;
using Microsoft.AspNetCore.Http;
using MYRAY.Business.Constants;
using MYRAY.Business.DTOs;
using MYRAY.Business.Exceptions;

namespace MYRAY.Business.Helpers.Paging;

public static class PagingHelper
{
    public static ResponseDto.CollectiveResponse<T> GetWithPaging<T, TEntity>(this IQueryable<TEntity>? query,
        PagingDto pagingDto, IMapper mapper) 
    where T: class
    where TEntity : class
    {
        if (pagingDto.PageSize > PagingConstant.FixPagingConstant.MaxPageSize)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Input page size is over max size limitation.",
                nameof(pagingDto));
        }

        if (query == null)
        {
            return null!;
        }

        ResponseDto.CollectiveResponse<T> result = new ResponseDto.CollectiveResponse<T>();
        result.PagingMetadata = new ResponseDto.PagingDto()
        {
            PageSize = pagingDto.PageSize,
            TotalCount = query.Count()
        };
        
        result.PagingMetadata.TotalPages = (int)Math.Ceiling(result.PagingMetadata.TotalCount / (double)pagingDto.PageSize);
        result.PagingMetadata.PageIndex = pagingDto.Page <= result.PagingMetadata.TotalPages
            ? pagingDto.Page
            : result.PagingMetadata.TotalPages;
        result.PagingMetadata.HasPreviousPage = result.PagingMetadata.PageIndex > 1;
        result.PagingMetadata.HasNextPage =
            result.PagingMetadata.PageIndex < result.PagingMetadata.TotalPages;
        IQueryable<TEntity> queryPaging = result.PagingMetadata.PageIndex <= 0
            ? query.Take(0)
            : query.Skip(result.PagingMetadata.PageIndex <= 1
                    ? 0
                    : result.PagingMetadata.PageSize * (result.PagingMetadata.PageIndex - 1))
                .Take(result.PagingMetadata.PageSize);
        result.ListObject = mapper.ProjectTo<T>(queryPaging);
        return result;
    }
}