using BookShop.API.Model.Entity;
using BookShop.API.Repository;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BookShop.API.Service;

public class UserService : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private IMongoCollection<User> _users => _context.MongoDatabase.GetCollection<User>("Users");

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }


    private IMongoCollection<BsonDocument> _library =>
        _context.MongoDatabase.GetCollection<BsonDocument>("UsersLibrary");

    private IMongoCollection<BsonDocument> _wishList =>
        _context.MongoDatabase.GetCollection<BsonDocument>("UsersWishlist");

    private bool CheckIfExist(User item)
    {
        var currentUser = _users.Find(u => u.Email == item.Email!.ToLower() ||
                                           u.Username == item.Username!.ToLower()).FirstOrDefault();

        if (currentUser != null)
            return true;

        return false;
    }

    public IEnumerable<User> GetList()
    {
        return _users.Find(_ => true).ToList();
    }

    public User GetItem(string id)
    {
        return _users.Find(u => u.Id == id).FirstOrDefault();
    }

    public List<Book> GetUserWishlist(string id)
    {
        var user = GetItem(id);
        if (CheckIfExist(user))
        {
            var filter = Builders<BsonDocument>.Filter.Eq("OwnerId", id);
            var result = _wishList.Find(filter).FirstOrDefault();
            return GetBookList(result["Wishlist"]);
        }

        return null;
    }

    public List<Book> GetUserLibrary(string id)
    {
        var user = GetItem(id);

        if (CheckIfExist(user))
        {
            var filter = Builders<BsonDocument>.Filter.Eq("OwnerId", id);
            var result = _library.Find(filter).FirstOrDefault();
            return GetBookList(result["BoughtBook"]);
        }

        return null!;
    }

    public List<Book> GetBookList(BsonValue searchedList)
    {
        return searchedList.AsBsonArray.Select(x => new Book
        {
            Id = x["_id"].AsString,
            SellerId = x["SellerId"].AsString,
            Title = x["Title"].AsString,
            Description = x["Description"].AsString,
            Genre = x["Genre"].AsBsonArray.Select(g => g.ToString()).ToArray(),
            Author = x["Author"].AsString,
            Price = x["Price"].AsDouble,
            YearOfPublication = x["YearOfPublication"].AsInt32,
            CountOfPages = x["CountOfPages"].AsInt32,
            CountInStock = x["CountInStock"].AsInt32
        }).ToList();
    }
    
    public bool CheckIfBookExistInWishList(string userId, string bookId)
    {
        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("OwnerId", userId),
            Builders<BsonDocument>.Filter.ElemMatch(
                "Wishlist", Builders<BsonDocument>.Filter.Eq("_id", bookId)
            ));
        var projection =
            Builders<BsonDocument>.Projection.ElemMatch("Wishlist", Builders<BsonDocument>.Filter.Eq("_id", bookId));
        var result = _wishList.Find(filter).Project(projection).FirstOrDefault();

        return result is not null ? true : false;
    }

    public bool CheckIfBookExistInLibrary(string userId, string bookId)
    {
        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("OwnerId", userId),
            Builders<BsonDocument>.Filter.ElemMatch(
                "BoughtBook", Builders<BsonDocument>.Filter.Eq("_id", bookId)
            ));
        var projection =
            Builders<BsonDocument>.Projection.ElemMatch("BoughtBook", Builders<BsonDocument>.Filter.Eq("_id", bookId));
        var result = _library.Find(filter).Project(projection).FirstOrDefault();

        return result is not null ? true : false;
    }

    public void AddBookToUserWishList(string id, Book book)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("OwnerId", id);
        var update = Builders<BsonDocument>.Update.Push("Wishlist", book);

        _wishList.FindOneAndUpdateAsync(filter, update);
    }

    public void DeleteBookFromUserWishList(string id, Book book)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("OwnerId", id);
        var update = Builders<BsonDocument>.Update.Pull("Wishlist", book);
        _wishList.FindOneAndUpdateAsync(filter, update);
    }

    public void AddBookToUserLibrary(string id, Book book)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("OwnerId", id);
        var update = Builders<BsonDocument>.Update.Push("BoughtBook", book);

        _library.FindOneAndUpdateAsync(filter, update);
    }

    public void Create(User user)
    {
        user.Id = Guid.NewGuid().ToString();
        _users.InsertOne(user);
    }

    public void Update(User item)
    {
        _users.ReplaceOne(Builders<User>.Filter.Eq("_id", item.Id), item);
    }

    public void Delete(string id)
    {
        var user = GetItem(id);
        if (user != null)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("OwnerId", id);
            _users.DeleteOne(u => u.Id == user.Id);
            _library.DeleteManyAsync(filter);
            _wishList.DeleteManyAsync(filter);
        }
    }

    public void Dispose()
    {
    }

    public void Save()
    {
    }
}