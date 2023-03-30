    namespace BookShop.API.Authorization;

public class RefreshTokenDocument
{
    public string? Id { get; set; } 
    public string UserId { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime TokenCreated { get; set; } = DateTime.Now;
    public DateTime TokenExpires { get; set; } = DateTime.Now.AddDays(7);
}