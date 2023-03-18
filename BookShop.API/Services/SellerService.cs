using BookShop.API.Model.Entity;
using MongoDB.Driver;

namespace BookShop.API.Service;

public class SellerService
{
    private readonly ApplicationDbContext _context;
    private IMongoCollection<User> _users => _context.MongoDatabase.GetCollection<User>("Users");
    private UserService _userService;
    public SellerService(ApplicationDbContext context, UserService userService)
    {
        _userService = userService;
        _context = context;
    }
    
    public void UpdateCountOfProduct(string sellerId)
    {
        var currentSeller = (Seller)_userService.GetItem(sellerId);
        currentSeller.CountOfProduct++;
        var filter = Builders<User>.Filter.Eq("_id", sellerId);
        var update = Builders<User>.Update.Set("CountOfProduct", currentSeller.CountOfProduct++);
        _users.UpdateOne(filter, update);
    }

    public void UpdateCountOfSoldProduct(string sellerId, int countOfProduct)
    {
        var currentSeller = (Seller)_userService.GetItem(sellerId);
        var filter = Builders<User>.Filter.Eq("_id", currentSeller.Id);
        var update = Builders<User>.Update.Set("CountOfSoldProduct", currentSeller.CountOfSoldProduct += countOfProduct);
        _users.UpdateOne(filter, update);
    }
}