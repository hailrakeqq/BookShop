using BookShop.API.Model.Entity;

namespace BookShop.API.Repository;

public interface ICommentRepository : IRepository<Comment>
{
    bool IsUserHaveCurrentComment(Comment comment, string id);
    List<Comment> GetUserComment(string userId);
    List<Comment> GetBookComment(string bookId);
}