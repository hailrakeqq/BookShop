using BookShop.API.Model.Entity;
using BookShop.API.Repository;
using MongoDB.Driver;

namespace BookShop.API.Service;

public class SellerService : ISellerRepository
{
    private readonly ApplicationDbContext _context;
    private IMongoCollection<User> _users => _context.MongoDatabase.GetCollection<User>("Users");
    private IMongoCollection<SellerStats> _sellerStats =>
        _context.MongoDatabase.GetCollection<SellerStats>("SellerStats");
    public SellerService(ApplicationDbContext context)
    {
        _context = context;
    }

    public SellerStats GetSellerStats(string id)
    {
        var sellerStats = _sellerStats.Find(st => st.SellerId == id).FirstOrDefault();
        return sellerStats != null ? sellerStats : null!;
    }

    public void DeleteSellerStats(string id)
    {
        _sellerStats.DeleteOne(Builders<SellerStats>.Filter.Eq("SellerId", id));
    }

    public void UpdateCountOfProductOnAdd(string sellerId)
    {
        var filter = Builders<SellerStats>.Filter.Eq("SellerId", sellerId);
        var currentSellerStats = _sellerStats.Find(filter).FirstOrDefault();
        var update = Builders<SellerStats>.Update.Set("CountOfProduct", currentSellerStats.CountOfProduct + 1);
        _sellerStats.UpdateOne(filter, update);
    }
    
    public void UpdateCountOfProductOnDelete(string sellerId)
    {
        var filter = Builders<SellerStats>.Filter.Eq("SellerId", sellerId);
        var currentSellerStats = _sellerStats.Find(filter).FirstOrDefault();
        var update = Builders<SellerStats>.Update.Set("CountOfProduct", currentSellerStats.CountOfProduct - 1);
        _sellerStats.UpdateOne(filter, update);
    }

    public void UpdateCountOfSoldProduct(string sellerId, int countOfProduct)
    {
        var currentSellerStats = _sellerStats.Find(u => u.SellerId == sellerId).FirstOrDefault();
        var filter = Builders<SellerStats>.Filter.Eq("SellerId", sellerId);
        var update = Builders<SellerStats>.Update.Set("CountOfSoldProduct", currentSellerStats.CountOfSoldProduct += countOfProduct);
        _sellerStats.UpdateOne(filter, update);
    }
}