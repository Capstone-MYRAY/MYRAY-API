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
        /// Đã xóa
        /// </summary>
        Deleted = 0,

        /// <summary>
        /// Quá hạn
        /// </summary>
        OutOfDate = 5,

        /// <summary>
        /// Đã hủy
        /// </summary>
        Cancel = 6,

        /// <summary>
        /// Đã duyệt
        /// </summary>
        Approved = 7,

        /// <summary>
        /// 
        /// </summary>
        ShortHandled = 8,

        /// <summary>
        /// Tuyển đủ người
        /// </summary>
        Enough = 9,
        
        /// <summary>
        /// Kết thúc
        /// </summary>
        End = 10
    }

    public enum JobPostWorkStatus
    {
        Pending = 0,
        Started = 1,
        Done = 2
    }

    public enum JobPostSortCriteria
    {
        CreatedDate,
        ApprovedDate
    }
}