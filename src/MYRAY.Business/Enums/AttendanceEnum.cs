namespace MYRAY.Business.Enums;

public class AttendanceEnum
{
    public enum AttendanceStatus
    {
        /// <summary>
        /// Có mặt
        /// </summary>
        Present,
        
        /// <summary>
        /// Chưa đến ngày điểm danh
        /// </summary>
        Future,
        
        /// <summary>
        /// Vắng mặt
        /// </summary>
        Absent,
        
        /// <summary>
        /// Kết thúc
        /// </summary>
        End,
        
        /// <summary>
        /// Xin nghỉ
        /// </summary>
        DayOff,
        
        /// <summary>
        /// Sa thải
        /// </summary>
        Dismissed,
        
        /// <summary>
        /// Hoàn thành
        /// </summary>
        Done
        
    }
}