using System.ComponentModel;

namespace MYRAY.Business.DTOs.Account;

public class SearchAccountDto
{
    [DefaultValue("")] public string Fullname { get; set; } = "";
    [DefaultValue("")] public string PhoneNumber { get; set; } = "";
    [DefaultValue("")] public string? Address { get; set; }="";
    [DefaultValue("")] public string? Email { get; set; }  ="";
    [DefaultValue(null)] public int? RoleId { get; set; } = null;
    [DefaultValue(null)] public int? Status { get; set; } = null;
}