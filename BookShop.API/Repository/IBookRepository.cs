using BookShop.API.Model.Entity;

namespace BookShop.API.Repository;

public interface IBookRepository : IRepository<Book>
{
    bool isBookExistWithCurrentTitleFindByItem(Book item);
    bool IsBookExistFindById(string id);
    void UpdateOnBuy(Book book, int countBooks);
    void UpdateBookRating(string id, double rating);
    double GetCurrentBookRating(string id);
}