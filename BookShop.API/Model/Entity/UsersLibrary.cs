namespace BookShop.API.Model.Entity;

public class UsersLibrary
{
    public string? Id { get; set; }
    public string? OwnerId { get; set; }
    public HashSet<Book>? BoughtBook { get; set; } //test what is better for save HashSet or List
}