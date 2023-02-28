using BookShop.API.Model.Entity;

namespace BookShop.API.Repository;

public interface IMongoUserRepository : IRepository<User>
{
    bool CheckIfExist(User item);
    Task<List<Book>> GetUserWishListOrLibrary(string typeOfRequest, string id);
    bool CheckIfBookExistInWishList(string userId, string bookId);
    bool CheckIfBookExistInLibrary(string userId, string bookId);
    void AddBookToUserWishList(string id, Book book);
    void DeleteBookFromUserWishList(string id, Book book);
    void AddBookToUserLibrary(string id, Book book);
}