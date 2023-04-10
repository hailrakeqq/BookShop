using BookShop.API;
using BookShop.API.Authorization;
using BookShop.API.Controllers;
using BookShop.API.Model.Entity;
using BookShop.API.Repository;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Moq;

namespace BookShop.Test.ControllersTest;

public class AuthControllerTest
{
    private readonly IMongoDatabase _database;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMongoCollection<SellerStats>> _sellerStatsCollectionMock;
    private readonly Mock<IMongoCollection<User>> _usersCollectionMock;
    private readonly Mock<IMongoCollection<RefreshTokenDocument>> _tokenCollection;
    private readonly Mock<IMongoCollection<UsersLibrary>> _usersLibraryCollectionMock;
    private readonly Mock<IMongoCollection<UsersWishlist>> _usersWishlistCollectionMock;
    private readonly IConfiguration config;
    private AuthController _authController;

    public AuthControllerTest()
    {
        config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        //new IConfiguration
        //var client = new MongoClient(configuration["ConnectionString"]);
        //_database = client.GetDatabase("BookShop");

        _tokenServiceMock = new Mock<ITokenService>();
        _userRepositoryMock = new Mock<IUserRepository>();

        _authController = new AuthController(new ApplicationDbContext(config), _tokenServiceMock.Object, _userRepositoryMock.Object);
    }

    [Fact]
    public async void CreateUser_ShouldCreateUserAndReturnOkResult_WhenUserDoesNotExist()
    {
        // Arrange
        var user = new User
        {
            Email = "testuser@example.com",
            Username = "testuser",
            Password = "password",
            Role = "user"
        };

        // Act
        var result = await _authController.CreateUser(user) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(200);
        var createdUser = result.Value as User;
        createdUser.Should().NotBeNull();
        createdUser.Email.Should().Be(user.Email.ToLower());
        createdUser.Username.Should().Be(user.Username.ToLower());
        createdUser.Role.Should().Be(user.Role);

        // Check that the user was added to the database
        var users = await _usersCollectionMock.Object.FindAsync(u => u.Email == user.Email.ToLower() ||
                                                         u.Username == user.Username.ToLower());
        var createdUserInDb = await users.FirstOrDefaultAsync();
        createdUserInDb.Should().NotBeNull();
        createdUserInDb.Email.Should().Be(user.Email.ToLower());
        createdUserInDb.Username.Should().Be(user.Username.ToLower());
        createdUserInDb.Password.Should().NotBe(user.Password); // check that the password was hashed
    }

    [Fact]
    public void Login_ShouldReturn200Result()
    {
        //Arrange 
        var loginUserData = new UserLoginModel()
        {
            EmailOrLogin = "test",
            Password = "1"
        };

        //Act
        var result = _authController.Login(loginUserData) as OkObjectResult;

        //Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(200);
        var loginResponse = result.Value as User;
        loginResponse.Should().NotBeNull();
    }
}