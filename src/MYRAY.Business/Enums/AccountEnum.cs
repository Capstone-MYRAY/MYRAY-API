namespace MYRAY.Business.Enums;

public class AccountEnum
{
    /// <summary>
    /// Enum for status account.
    /// </summary>
    public enum AccountStatus
    {
        /// <summary>
        /// Status for active account
        /// </summary>
        Active = 1,
        
        /// <summary>
        /// Status for Deleted account.
        /// </summary>
        Inactive = 0,
        
        /// <summary>
        /// Status for banned account
        /// </summary>
        Banned = 2
    }

    /// <summary>
    /// Enum for sort criteria
    /// </summary>
    public enum AccountSortCriteria
    {
        /// <summary>
        /// Fullname attr.
        /// </summary>
        Fullname,
        
        /// <summary>
        /// Date Of birth attr.
        /// </summary>
        DateOfBirth,
        
        /// <summary>
        /// Gender attr.
        /// </summary>
        Gender,
        
        /// <summary>
        /// Balance attr.
        /// </summary>
        Balance,
        
        /// <summary>
        /// Point attr.
        /// </summary>
        Point
    }
}