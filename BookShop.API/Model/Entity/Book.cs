namespace BookShop.API.Model.Entity;

public class Book
{
    public string? Id { get; set; }
    public string? SellerId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string[]? Genre { get; set; }
    public string? Author { get; set; }
    public double Price { get; set; }
    public int? YearOfPublication { get; set; }
    public int? CountOfPages { get; set; }
    public int CountInStock { get; set; }
}
