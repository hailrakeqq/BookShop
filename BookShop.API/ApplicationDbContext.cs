using MongoDB.Driver;

namespace BookShop.API;

public class ApplicationDbContext
{
    internal object users;

    public ApplicationDbContext(IConfiguration config)
    {
        var client = new MongoClient(config["ConnectionString"]);

        MongoDatabase = client.GetDatabase("BookShop");
    }

    public IMongoDatabase MongoDatabase { get; set; }
}