using BookShop.API.Model.Entity;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BookShop.API.Repository;

public class UserService : IMongoUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    private IMongoCollection<User> _users => _context.MongoDatabase.GetCollection<User>("Users");

    private IMongoCollection<BsonDocument> _library =>
        _context.MongoDatabase.GetCollection<BsonDocument>("UsersLibrary");

    private IMongoCollection<BsonDocument> _wishList =>
        _context.MongoDatabase.GetCollection<BsonDocument>("UsersWishlist");

    public bool CheckIfExist(User item)
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

    public async Task<List<Book>> GetUserWishListOrLibrary(string typeOfRequest, string id)
    {
        var user = GetItem(id);
        if (user != null)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("OwnerId", id);

            if (typeOfRequest == "GetUserWishList")
            {
                var result = _wishList.Find(filter).FirstOrDefault();
                return result["Wishlist"].AsBsonArray.Select(x => new Book
                {
                    Id = x["_id"].AsString,
                    SellerId = x["SellerId"].AsString,
                    Title = x["Title"].AsString,
                    Description = x["Description"].AsString,
                    Genre = x["Genre"].AsString,
                    Author = x["Author"].AsString,
                    Price = x["Price"].AsDouble,
                    YearOfPublication = x["YearOfPublication"].AsInt32,
                    CountOfPages = x["CountOfPages"].AsInt32,
                    CountInStock = x["CountInStock"].AsInt32
                }).ToList();
            }
            else
            {
                var result = _library.Find(filter).FirstOrDefault();
                return result["BoughtBook"].AsBsonArray.Select(x => new Book
                {
                    Id = x["_id"].AsString,
                    SellerId = x["SellerId"].AsString,
                    Title = x["Title"].AsString,
                    Description = x["Description"].AsString,
                    Genre = x["Genre"].AsString,
                    Author = x["Author"].AsString,
                    Price = x["Price"].AsDouble,
                    YearOfPublication = x["YearOfPublication"].AsInt32,
                    CountOfPages = x["CountOfPages"].AsInt32,
                    CountInStock = x["CountInStock"].AsInt32
                }).ToList();
            }
        }

        return null!;
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
        _users.ReplaceOne(item.Id, item);
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