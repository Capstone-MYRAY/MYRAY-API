namespace MYRAY.Business.Enums;

public class AreaEnum
{
    /// <summary>
    /// Enum for Status Area
    /// </summary>
    public enum AreaStatus
    {
        /// <summary>
        /// Status for Active Area
        /// </summary>
        Active = 1,
        
        /// <summary>
        /// Status for Deleted Area
        /// </summary>
        Inactive = 0
    }
    
    public enum AreaSortCriteria
    {
        /// <summary>
        /// Province attr.
        /// </summary>
        Province,
        
        /// <summary>
        /// District attr.
        /// </summary>
        District,
        
        /// <summary>
        /// Commune attr.
        /// </summary>
        Commune
    }
}