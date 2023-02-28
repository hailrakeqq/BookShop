using BookShop.API.Model.Entity;

namespace BookShop.Test.TestsData;

public class BookData
{
    public static List<Book> GetBooks()
    {
        return new List<Book>
        {
            new()
            {
                Id = "book1",
                SellerId = "testidfortest",
                Title = "book1",
                CountInStock = 100
            },
            new()
            {
                Id = "book2",
                SellerId = "testidfortest",
                Title = "book2",
                CountInStock = 50
            }
        };
    }
}