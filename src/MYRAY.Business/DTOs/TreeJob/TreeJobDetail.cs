using MYRAY.Business.DTOs.TreeType;

namespace MYRAY.Business.DTOs.TreeJob;

public class TreeJobDetail
{
    public int TreeTypeId { get; set; }
    public int JobPostId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public TreeTypeDetail TreeType { get; set; } = null!;
}