using BookShop.API.Model.Entity;

namespace BookShop.API.Repository;

public interface IBookRepository : IRepository<Book>
{
    bool isBookExistWithCurrentTitleFindByItem(Book item);
    bool IsBookExistFindById(string id);
    void UpdateOnBuy(Book book, int countBooks);
}