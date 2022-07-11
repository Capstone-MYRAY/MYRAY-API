namespace MYRAY.Business.Enums;

public class AppliedJobEnum
{
    public enum AppliedJobStatus
    {
        Approve = 1,
        Pending = 0,
        Reject = 2,
        End = 3
    }
    
    public enum SortCriteriaAppliedJob
    {
        AppliedDate,
        ApprovedDate,
        StartDate,
        EndDate
    }
}