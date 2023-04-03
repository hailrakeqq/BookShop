using BookShop.API.Model.Entity;

namespace BookShop.API.Repository;

public interface IUserRepository : IRepository<User>
{
    List<SellerPublicData> GetSellerList();
    List<Book> GetUserWishlist(string id);
    List<Book> GetUserLibrary(string id);
    bool CheckIfBookExistInWishList(string userId, string bookId);
    Task<bool>  CheckIfBookExistInLibrary(string userId, string bookId);
    void AddBookToUserWishList(string id, Book book);
    void DeleteBookFromUserWishList(string id, Book book);
    void AddBookToUserLibrary(string id, Book book);
}