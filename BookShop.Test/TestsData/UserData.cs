using BookShop.API.Authorization;
using BookShop.API.Model.Entity;

namespace BookShop.Test.TestsData;

public class UserData
{
    private static readonly User user = new()
    {
        Id = "current",
        Username = "TestCurrent",
        Email = "current@mail.com",
        Role = "user",
        CountOfProduct = 0
    };

    public static CurrentUser currentUser =
        new()
        {
            User = user
        };

    public static List<User> GetUsers()
    {
        return new List<User>
        {
            new()
            {
                Id = "testidfortest",
                Username = "TestSeller",
                Email = "Seller@mail.com",
                Role = "seller",
                CountOfProduct = 0
            },
            new()
            {
                Id = "user1",
                Username = "TestUser1",
                Email = "user1@mail.com",
                Role = "user",
                CountOfProduct = 0
            },
            new()
            {
                Id = "user2",
                Username = "TestUser2",
                Email = "user2@mail.com",
                Role = "user",
                CountOfProduct = 0
            }
        };
    }
}