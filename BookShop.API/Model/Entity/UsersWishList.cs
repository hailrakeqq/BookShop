namespace BookShop.API.Model.Entity;

public class UsersWishlist
{
    public string? Id { get; set; }
    public string? OwnerId { get; set; }
    public HashSet<Book>? Wishlist { get; set; } //test what is better for save HashSet or List
}