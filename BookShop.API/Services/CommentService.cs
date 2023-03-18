using BookShop.API.Model.Entity;
using BookShop.API.Repository;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BookShop.API.Service;

public class CommentService : ICommentRepository
{
    
    private readonly ApplicationDbContext _context;
    private IMongoCollection<Comment> _comments => _context.MongoDatabase.GetCollection<Comment>("Comments");

    public CommentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Comment> GetUserComment(string userId)
    {
        return null;
    }

    public IEnumerable<Comment> GetList()
    {
        return _comments.Find(_ => true).ToList();
    }

    public Comment GetItem(string id)
    {
        throw new NotImplementedException();
    }

    public void Create(Comment item)
    {
        throw new NotImplementedException();
    }

    public void Update(Comment item)
    {
        throw new NotImplementedException();
    }

    public void Delete(string id)
    {
        throw new NotImplementedException();
    }

    public void Save()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
