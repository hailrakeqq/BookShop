using BookShop.API.Model.Entity;

namespace BookShop.API.Repository;

public interface ICommentRepository : IRepository<Comment>
{
    List<Comment> GetUserComment(string userId);
    
}