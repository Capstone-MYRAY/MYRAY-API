namespace MYRAY.Business.Enums;

public class AttendanceEnum
{
    public enum AttendanceStatus
    {
        /// <summary>
        /// Có mặt
        /// </summary>
        Present = 1,
        
        /// <summary>
        /// Chưa đến ngày điểm danh
        /// </summary>
        Future = 2,
        
        /// <summary>
        /// Vắng mặt không phép
        /// </summary>
        UnexcusedAbsent = 0,
        
        /// <summary>
        /// Kết thúc
        /// </summary>
        End = 3,
        
        /// <summary>
        /// Xin nghỉ
        /// </summary>
        DayOff = 4,
        
        /// <summary>
        /// Sa thải
        /// </summary>
        Dismissed = 5,
        
        /// <summary>
        /// Vắng có phép
        /// </summary>
        ExcussedAbsence = 6
        
    }
}