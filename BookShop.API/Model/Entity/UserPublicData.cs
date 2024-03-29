namespace BookShop.API.Model.Entity;

public class UserPublicData
{
    public string Id { get; set; }
    public string? Username { get; set; }
    public string? Role { get; set; }
    public int BoughtBook { get; set; }
    public int WishlistCount { get; set; }
}

