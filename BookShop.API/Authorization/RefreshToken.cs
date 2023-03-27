using MongoDB.Driver.Linq;

namespace BookShop.API.Authorization;

public class RefreshToken
{
    public string Token { get; set; } = string.Empty;
    public DateTime Created { get; } = DateTime.Now;
    public DateTime Expires { get; } = DateTime.Now.AddDays(7);
}