namespace MYRAY.Business.Enums;

public class ReportEnum
{
    public enum ReportStatus
    {
        Pending = 1,
        Resolved = 2,
        Deleted = 0
    }

    public enum ReportSortCriterial
    {
        CreatedDate,
        ReportedId,
        JobPostId
    }
}