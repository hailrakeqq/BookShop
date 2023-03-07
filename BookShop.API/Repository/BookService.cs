using BookShop.API.Model.Entity;
using MongoDB.Driver;

namespace BookShop.API.Repository;

public class BookService : IMongoBookRepository
{
    private readonly ApplicationDbContext _context;

    public BookService(ApplicationDbContext context)
    {
        _context = context;
    }

    private IMongoCollection<Book> _books => _context.MongoDatabase.GetCollection<Book>("Books");

    public bool CheckIfExist(Book item)
    {
        // var existBook = _books.Find(u => u.Title == item.Title).FirstOrDefault();
        var filter = Builders<Book>.Filter.Eq("title", item.Title);
        var existBook = _books.Find(filter).FirstOrDefault();
        if (existBook != null)
            return true;

        return false;
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
        _books.InsertOne(item);
    }

    public void Update(Book item)
    {
        _books.ReplaceOne(item.Id, item);
    }

    public void UpdateOnBuy(Book book, int countBooks)
    {
        var filter = Builders<Book>.Filter.Eq("Id", book.Id);
        var update = Builders<Book>.Update.Set("CountInStock", book.CountInStock - countBooks);
        _books.UpdateOne(filter, update);
    }

    public void Delete(string id)
    {
        var book = GetItem(id);
        if (book != null) _books.DeleteOne(u => u == book);
    }

    public void Dispose()
    {
    }

    public void Save()
    {
    }
}