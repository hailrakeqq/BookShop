namespace BookShop.API.Model.Entity;

public class Seller : User
{
    public int CountOfProduct { get; set; } 
    public int CountOfSoldProduct { get; set; }
}