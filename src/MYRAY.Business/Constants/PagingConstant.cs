namespace MYRAY.Business.Constants;
/// <summary>
/// Contains constant classes related to Pagination.
/// </summary>
public static class PagingConstant
{
    /// <summary>
    /// Contains property length related to Paging
    /// </summary>
    public static class FixPagingConstant
    {
        /// <summary>
        /// Default of Page Index
        /// </summary>
        public const int DefaultPage = 1;
        
        /// <summary>
        /// Default item in a page.
        /// </summary>
        public const int DefaultPageSize = 20;
        
        /// <summary>
        /// Default sort criteria.
        /// </summary>
        public const string DefaultSort = "id_asc";
        
        /// <summary>
        /// Limitation Min Page
        /// </summary>
        public const int MinPage = 0;
        
        /// <summary>
        /// Limitation min item of a page.
        /// </summary>
        public const int MinPageSize = 3;
        
        /// <summary>
        /// Limitation Max Page Size
        /// </summary>
        public const int MaxPageSize = 250;
        
        
        
    }

    public enum OrderCriteria
    {
        /// <summary>
        /// ascendant
        /// </summary>
        ASC,
        
        /// <summary>
        /// descendant
        /// </summary>
        DESC
    }
}