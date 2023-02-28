using BookShop.API.Authorization;
using BookShop.API.Controllers;
using BookShop.API.Repository;
using BookShop.Test.TestsData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BookShop.Test.ControllersTest;

public class BookControllerTests
{
    private static readonly CurrentUser currentUser = UserData.currentUser;
    private readonly Mock<IMongoBookRepository> _mongoBookRepository = new();
    private readonly Mock<IMongoUserRepository> _mongoUserRepository = new();
    private readonly BookController systemUnderTests;

    public BookControllerTests()
    {
        systemUnderTests = new BookController(_mongoBookRepository.Object, _mongoUserRepository.Object);
    }

    // bool CheckIfExist(Book item);
    // T GetItem(string id); - ready
    // BuyBook((CurrentUser currentUser, [FromRoute] string id, [FromBody] int countBooks))
    //AddBookToWishList(CurrentUser currentUser, [FromRoute] string bookId)
    //DeleteBookFromWishList(CurrentUser currentUser, string bookId)
    [Fact]
    public async void GetBookList_ShouldReturn200Status()
    {
        // Arrange
        _mongoBookRepository.Setup(_ => _.GetList()).Returns(BookData.GetBooks());
        var systemUnderTests = new BookController(_mongoBookRepository.Object, _mongoUserRepository.Object);

        // Act
        var result = systemUnderTests.GetBookList();

        // Assert
        result.GetType().Should().Be(typeof(OkObjectResult));
        (result as OkObjectResult).StatusCode.Should().Be(200);
    }

    [Fact]
    public async void GetBookById_ShouldReturn200Status()
    {
        //Arrange
        var item = _mongoBookRepository.Setup(_ => _.GetItem("book1")).Returns(BookData.GetBooks()
            .Where(i => i.Id == "book1").FirstOrDefault()!);

        //Act 
        var result = systemUnderTests.GetBookById("book1");

        //Assert 
        result.GetType().Should().Be(typeof(OkObjectResult));
        (result as OkObjectResult).StatusCode.Should().Be(200);
    }

    [Fact]
    public async void GetBookById_ShouldReturn404Status()
    {
        //Arrange
        var item = _mongoBookRepository.Setup(_ => _.GetItem("book1523")).Returns(BookData.GetBooks()
            .Where(i => i.Id == "book1523").FirstOrDefault()!);

        //Act 
        var result = systemUnderTests.GetBookById("book1523");

        //Assert 
        result.GetType().Should().Be(typeof(NotFoundResult));
        (result as NotFoundResult).StatusCode.Should().Be(404);
    }

    [Fact]
    public async void BuyBook_ShouldReturn200Status()
    {
        // BuyBook((CurrentUser currentUser, [FromRoute] string id, [FromBody] int countBooks))
        //Arrange

        //Act 
        var result = systemUnderTests.BuyBook(currentUser, "book1", 2);

        //Assert
        result.GetType().Should().Be(typeof(OkObjectResult));
        (result as OkObjectResult).StatusCode.Should().Be(200);
    }
}