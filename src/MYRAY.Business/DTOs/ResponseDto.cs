namespace MYRAY.Business.DTOs;

public static class ResponseDto
{
    public class ErrorResponse 
    {
        public string? Message { get; set; }
        public object? Target { get; set; }
    }

    public class CollectiveResponse<T> 
        where T : class
    {
        public IEnumerable<T> ListObject { get; set; }
        public PagingDto? PagingMetadata { get; set; }
    }

    public class PagingDto
    {
        /// <summary>
        /// Gets or sets page number.
        /// </summary>
        public int PageIndex { get; set; }
        
        /// <summary>
        /// Gets or sets number of record per page.
        /// </summary>
        public int PageSize { get; set; }
        
        /// <summary>
        /// Gets or sets total records of the whole list before paging.
        /// </summary>
        public int TotalCount { get; set; }
        
        /// <summary>
        /// Gets or sets total page after paging.
        /// </summary>
        public int TotalPages { get; set; }
        
        /// <summary>
        /// Gets status of checking has previous page.
        /// </summary>
        public bool HasPreviousPage { get; set; }
        
        /// <summary>
        /// Gets status of checking has next page.
        /// </summary>
        public bool HasNextPage { get; set; }
    }
}