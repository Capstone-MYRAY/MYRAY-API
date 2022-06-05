namespace MYRAY.Business.DTOs.Role;

public class GetRoleDetail
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}