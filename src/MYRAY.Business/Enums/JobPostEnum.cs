namespace MYRAY.Business.Enums;

public class JobPostEnum
{
    public enum JobPostStatus
    {
        /// <summary>
        /// Chờ duyệt
        /// </summary>
        Pending = 1,
        
        /// <summary>
        /// Đang đăng tải
        /// </summary>
        Posted = 2,
        
        /// <summary>
        /// Bị từ chối
        /// </summary>
        Reject = 3,
        
        /// <summary>
        /// Hết Hạn
        /// </summary>
        Expired = 4,
        
        /// <summary>
        /// Đã hủy
        /// </summary>
        Cancel = 0,
        
        /// <summary>
        /// Quá hạn
        /// </summary>
        OutOfDate = 5,
        
        Start = 6
    }
    
    public enum JobPostSortCriteria
    {
        PublishedDate,
        CreatedDate,
        ApprovedDate
    }
}