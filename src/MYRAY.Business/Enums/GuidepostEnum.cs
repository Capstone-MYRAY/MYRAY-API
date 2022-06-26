namespace MYRAY.Business.Enums;

public class GuidepostEnum
{
    public enum GuidepostStatus
    {
        /// <summary>
        /// Status for active.
        /// </summary>
        Active = 1,
        
        /// <summary>
        /// Status for inactive.
        /// </summary>
        Inactive = 0
    }
    
    public enum GuidepostSortCriteria
    {
        /// <summary>
        /// Title attr.
        /// </summary>
        Title,
        
        /// <summary>
        /// Content attr.
        /// </summary>
        Content,
        
        /// <summary>
        /// Create Date attr.
        /// </summary>
        CreatedDate
    }
}