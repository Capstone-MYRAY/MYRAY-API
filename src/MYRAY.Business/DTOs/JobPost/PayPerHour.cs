namespace MYRAY.Business.DTOs.JobPost;

public class PayPerHour
{
    public int EstimatedTotalTask { get; set; }
    public int MinFarmer { get; set; }
    public int MaxFarmer { get; set; }
    public double Salary { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan FinishTime { get; set; }
}