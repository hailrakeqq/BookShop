using BookShop.API.Controllers;
using BookShop.API.Model.Entity;
using BookShop.API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Test.ControllersTest;

public class UserControllerTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ISellerRepository> _sellerRepositoryMock;
    private readonly Mock<IFileRepository> _fileRepositoryMock;
    private readonly UserController _userController;

    public UserControllerTests()
    {
        _bookRepositoryMock = new Mock<IBookRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _fileRepositoryMock = new Mock<IFileRepository>();
        _sellerRepositoryMock = new Mock<ISellerRepository>();

        _userController = new UserController(
            _userRepositoryMock.Object,
            _sellerRepositoryMock.Object,
            _fileRepositoryMock.Object,
            _bookRepositoryMock.Object
            );
    }

    [Fact]
    public async void GetUserPublicData_ShouldReturn200Result()
    {
        // Arrange
        var id = "1";
        var user = new User()
        {
            Id = id,
            Username = "testuser",
            Role = "user"
        };
        var library = new List<Book>() { new Book() };
        var wishlist = new List<Book>() { new Book() };
        _userRepositoryMock.Setup(repo => repo.GetItem(id)).Returns(user);
        _userRepositoryMock.Setup(repo => repo.GetUserLibrary(id)).Returns(library);
        _userRepositoryMock.Setup(repo => repo.GetUserWishlist(id)).Returns(wishlist);

        // Act
        var response = _userController.GetUserPublicData(id) as OkObjectResult;
        var result = response.Value as UserPublicData;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Username, result.Username);
        Assert.Equal(user.Role, result.Role);
        Assert.Equal(library.Count, result.BoughtBook);
        Assert.Equal(wishlist.Count, result.WishlistCount);
        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }

    [Fact]
    public void GetPublicUserData_ShouldReturn200Result()
    {
        //Arrange
        var id = "2";
        var user = new User()
        {
            Id = id,
            Email = "test@email.com",
            Username = "testseller",
            Role = "seller"
        };
        var sellerStats = new SellerStats() { CountOfProduct = 10, SellerId = id, CountOfSoldProduct = 200 };

        _userRepositoryMock.Setup(repo => repo.GetItem(id)).Returns(user);
        _sellerRepositoryMock.Setup(repo => repo.GetSellerStats(id)).Returns(sellerStats);

        //Act
        var response = _userController.GetUserPublicData(id) as OkObjectResult;
        var result = response.Value as SellerPublicData;

        //Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(user.Username, result.Username);
        Assert.Equal(user.Role, result.Role);
        Assert.Equal(sellerStats.CountOfProduct, result.CountOfProduct);
        Assert.Equal(sellerStats.CountOfSoldProduct, result.CountOfSoldProduct);
        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }

    [Fact]
    public void GetPublicUserData_ReturnsNotFound_WhenUserNotFound()
    {
        // Arrange
        var id = "3";
        _userRepositoryMock.Setup(repo => repo.GetItem(id)).Returns((User)null);

        // Act
        var response = _userController.GetUserPublicData(id) as NotFoundResult;

        // Assert
        Assert.NotNull(response);
        Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
    }
}
