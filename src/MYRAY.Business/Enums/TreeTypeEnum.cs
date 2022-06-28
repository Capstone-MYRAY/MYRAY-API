namespace MYRAY.Business.Enums;

public class TreeTypeEnum
{
    /// <summary>
    /// Enum for tree type
    /// </summary>
    public enum TreeTypeStatus
    {
        /// <summary>
        /// Status for active tree type
        /// </summary>
        Active = 1,
        
        /// <summary>
        /// Status for inactive tree type
        /// </summary>
        Inactive = 0
    }
    
    public enum TreeTypeSortCriteria
    {
        /// <summary>
        /// Type attr.
        /// </summary>
        Type,
        
        Status
    }
}