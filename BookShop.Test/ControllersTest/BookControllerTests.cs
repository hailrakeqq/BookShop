using BookShop.API.Controllers;
using BookShop.API.Model.Entity;
using BookShop.API.Repository;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BookShop.Test.ControllersTest;

public class BookControllerTests
{
    private readonly Mock<IBookRepository> _bookRepository;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<ISellerRepository> _sellerRepository;
    private readonly BookController _bookController;

    public BookControllerTests()
    {
        var mockBookRepository = new Mock<IBookRepository>();
        var mockUserRepository = new Mock<IUserRepository>();
        var mockSellerRepository = new Mock<ISellerRepository>();

        _bookRepository = mockBookRepository;
        _userRepository = mockUserRepository;
        _sellerRepository = mockSellerRepository;

        _bookController = new BookController(_bookRepository.Object, _userRepository.Object, _sellerRepository.Object);
    }

    [Fact]
    public async void GetBookList_ShouldReturn200Status()
    {
        // Arrange

        // Act
        var result = _bookController.GetBookList();

        // Assert
        result.GetType().Should().Be(typeof(OkObjectResult));
        (result as OkObjectResult).StatusCode.Should().Be(200);
    }

    [Fact]
    public async void GetBookById_ShouldReturn200Status()
    {
        //Arrange
        var bookID = "50c460fa-263e-4389-954e-0d4e2ae3c355";
        _bookRepository.Setup(repo => repo.GetItem(bookID)).Returns(new Book { Id = bookID });

        //Act 
        var result = _bookController.GetBookById(bookID);

        //Assert 
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.StatusCode.Should().Be(200);
        var bookResponse = okResult.Value as Book;
        bookResponse.Id.Should().Be(bookID);
    }

    [Fact]
    public async void GetBookById_ShouldReturn404Status()
    {
        //Arrange

        //Act 
        var result = _bookController.GetBookById("test id");

        //Assert 
        result.GetType().Should().Be(typeof(NotFoundResult));
        (result as NotFoundResult).StatusCode.Should().Be(404);
    }

    [Fact]
    public async void BuyBook_ShouldReturn200Status()
    {
        //Arrange
        var currentUser = new API.Authorization.CurrentUser();
        int countBook = 1;
        string bookId = "50c460fa-263e-4389-954e-0d4e2ae3c355";

        //Act
        var result = await _bookController.BuyBook(currentUser, bookId, countBook);

        //Assert
        result.GetType().Should().Be(typeof(OkObjectResult));
        (result as OkObjectResult).StatusCode.Should().Be(200);
    }
}