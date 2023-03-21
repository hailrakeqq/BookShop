namespace BookShop.API.Model.Entity;

public class UserPublicData
{
    public string Id { get; set; }
    public string? Username { get; set; }
    public string? Role { get; set; }
    
    //only seller can have it
    public int CountOfProduct { get; set; }
    public int CountOfSoldProduct { get; set; }
    
    //only user can have it
    public int BoughtBook { get; set; }
    public int WishlistCount { get; set; }
}