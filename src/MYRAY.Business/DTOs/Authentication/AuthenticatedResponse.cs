namespace MYRAY.Business.DTOs.Authentication;

public class AuthenticatedResponse
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
}