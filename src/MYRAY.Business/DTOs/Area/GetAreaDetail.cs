namespace MYRAY.Business.DTOs.Area;

public class GetAreaDetail : InsertAreaDto
{
    public int Id { get; set; }
    public int? Status { get; set; }
}