using BookShop.API.Model.Entity;

namespace BookShop.API.Repository;

public interface IMongoBookRepository : IRepository<Book>
{
    bool CheckIfExist(Book item);
    void UpdateOnBuy(Book book, int countBooks);
}