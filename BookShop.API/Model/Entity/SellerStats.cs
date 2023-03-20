namespace BookShop.API.Model.Entity;

public class SellerStats
{
    public string? Id { get; set; }
    public string? SellerId { get; set; }
    public int CountOfProduct { get; set; }
    public int CountOfSoldProduct { get; set; }
}