using BookShop.API.Model.Entity;
using BookShop.API.Repository;
using MongoDB.Driver;

namespace BookShop.API.Service;

public class BookService : IBookRepository
{
    private readonly ApplicationDbContext _context;
    private IMongoCollection<Book> _books => _context.MongoDatabase.GetCollection<Book>("Books");
    private readonly ICommentRepository _commentRepository;
    public BookService(ApplicationDbContext context, ICommentRepository commentRepository)
    {
        _context = context;
        _commentRepository = commentRepository;
    }
    
    public IEnumerable<Book> GetList()
    {
        return _books.Find(_ => true).ToList();
    }

    public Book GetItem(string id)
    {
        return _books.Find(b => b.Id == id).FirstOrDefault();
    }

    public void Create(Book item)
    {
        item.Id = Guid.NewGuid().ToString();
        item.Rating = 0;
        _books.InsertOne(item);
    }

    public void Update(Book item)
    {
        _books.ReplaceOne(Builders<Book>.Filter.Eq("_id", item.Id), item);
    }

    public void UpdateOnBuy(Book book, int countBooks)
    {
        var filter = Builders<Book>.Filter.Eq("Id", book.Id);
        var update = Builders<Book>.Update.Set("CountInStock", book.CountInStock - countBooks);
        _books.UpdateOne(filter, update);
    }
    
    public void UpdateBookRating(string id ,double rating)
    {
        double ratingBeforeUpdate = GetCurrentBookRating(id);
        int commentCount = GetBookCommentCount(id); 
        double updatedRating = ((ratingBeforeUpdate * commentCount) + rating) / (commentCount + 1);
        
        _books.UpdateOne(
            Builders<Book>.Filter.Eq("_id", id),
            Builders<Book>.Update.Set("Rating", updatedRating));
    }

    public double GetCurrentBookRating(string id)
    {
        var bookCommentCollection = _commentRepository.GetBookComment(id);
        double rating = 0;
        
        foreach (var book in bookCommentCollection)
            rating += Convert.ToDouble(book.Rating);
        
        return rating / bookCommentCollection.Count;
    }

    private int GetBookCommentCount(string id)
    {
        return _commentRepository.GetBookComment(id).Count;
    }

    public void Delete(string id)
    {
        var book = GetItem(id);
        if (book != null) 
            _books.DeleteOne(Builders<Book>.Filter.Eq("_id", book.Id));
    }
    

    public bool isBookExistWithCurrentTitleFindByItem(Book item)
    {
        var existBook = _books.Find(Builders<Book>.Filter.Eq("title", item.Title)).FirstOrDefault();
        return existBook != null ? true : false;
    }

    public bool IsBookExistFindById(string id)
    {
        var existBook = _books.Find(Builders<Book>.Filter.Eq("_id", id)).FirstOrDefault();
        return existBook != null ? true : false;
    }

    public void Dispose()
    {
    }

    public void Save()
    {
    }
}