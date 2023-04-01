namespace BookShop.API.Model.Entity;
public class SellerPublicData
{
    public string Id { get; set; }
    public string? Username { get; set; }
    public string? Role { get; set; }
    public int CountOfProduct { get; set; }
    public int CountOfSoldProduct { get; set; }
}