namespace BookShop.API.Authorization;

public class LoginResponce
{
    public string? Id { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string? Role { get; set; }
    public string? JWTToken { get; set; }
    public string? RefreshToken { get; set; }
}