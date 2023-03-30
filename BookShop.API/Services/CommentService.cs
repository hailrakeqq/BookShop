using BookShop.API.Model.Entity;
using BookShop.API.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace BookShop.API.Service;

public class CommentService : ICommentRepository
{
    
    private readonly ApplicationDbContext _context;
    private IMongoCollection<Comment> _comments => _context.MongoDatabase.GetCollection<Comment>("Comments");

    public CommentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public bool IsUserHaveCurrentComment(Comment comment, string id)
    {
        return comment.UserId == id ? true : false;
    }

    public bool IsUserAlreadyHaveCommentForThisBook(string bookId, string userId)
    {
        var userComment = GetBookComment(bookId).Where(u => u.UserId == userId).FirstOrDefault();
        return userComment != null ? true : false;
    }
    
    public List<Comment> GetUserComment(string userId)
    {
        return GetList().Where(u => u.UserId == userId).ToList();
    }

    public IEnumerable<Comment> GetList()
    {
        return _comments.Find(_ => true).ToList();
    }

    public List<Comment> GetBookComment(string bookId)
    {
        var filter = Builders<Comment>.Filter.Eq("BookId", bookId);
        var commentCollection = _comments.Find(filter).ToList();

        return commentCollection != null ? commentCollection : null!;
    }

    public Comment GetItem(string id)
    {
        var filter = Builders<Comment>.Filter.Eq("_id", id);
        var comment = _comments.Find(filter).FirstOrDefault();

        return comment != null ? comment : null!;
    }

    public void Create(Comment item)
    {
        _comments.InsertOne(item);
    }

    public void Update(Comment item)
    {
        var filter = Builders<Comment>.Filter.Eq("_id", item.Id);
        var existComment = _comments.Find(filter).FirstOrDefault();
        
        if (existComment != null)
            _comments.ReplaceOne(filter, item);
    }

    public void Delete(string id)
    {
        var filter = Builders<Comment>.Filter.Eq("_id", id);
        var existComment = _comments.Find(filter).FirstOrDefault();

        if (existComment != null)
            _comments.DeleteOne(filter);
    }

    public void Save()
    {
    }

    public void Dispose()
    {
    }
}
