using BookShop.API.Authorization;
using BookShop.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class BookController : Controller
{
    private readonly IMongoBookRepository _bookRepository;
    private readonly IMongoUserRepository _userRepository;

    public BookController(IMongoBookRepository bookRepository, IMongoUserRepository userRepository)
    {
        _bookRepository = bookRepository;
        _userRepository = userRepository;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetBookList()
    {
        return Ok(_bookRepository.GetList().ToList());
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("GetBookById/{id:Guid}")]
    public IActionResult GetBookById(string id)
    {
        var book = _bookRepository.GetItem(id);

        if (book != null)
            return Ok(book);

        return NotFound();
    }

    [HttpPost]
    [Route("BuyBook/{id:Guid}")]
    public IActionResult BuyBook(CurrentUser currentUser, [FromRoute] string id, [FromBody] int countBooks)
    {
        var book = _bookRepository.GetItem(id);
        if (book.CountInStock > 0 && !_userRepository.CheckIfBookExistInLibrary(currentUser.Id, id))
        {
            if (_userRepository.CheckIfBookExistInWishList(currentUser.Id, id))
                _userRepository.DeleteBookFromUserWishList(currentUser.Id, book);
            _userRepository.AddBookToUserLibrary(currentUser.Id, book);
            _bookRepository.UpdateOnBuy(book, countBooks);
            return Ok(book);
        }

        return Conflict();
    }

    [HttpPost]
    [Route("AddBookToUserWishList/{id:Guid}")]
    public IActionResult AddBookToWishList(CurrentUser currentUser, [FromRoute] string id)
    {
        var existBook = _userRepository.CheckIfBookExistInWishList(currentUser.Id, id);
        if (!existBook)
        {
            _userRepository.AddBookToUserWishList(currentUser.Id, _bookRepository.GetItem(id));
            return Ok("Book has been add to your wish list");
        }

        return Conflict("Book is already exist in your wish list");
    }

    [HttpDelete]
    [Route("DeleteBookFromUserWishList/{id:Guid}")]
    public IActionResult DeleteBookFromWishList(CurrentUser currentUser, string id)
    {
        var existBook = _userRepository.CheckIfBookExistInWishList(currentUser.Id, id);
        if (existBook)
        {
            _userRepository.DeleteBookFromUserWishList(currentUser.Id, _bookRepository.GetItem(id));
            return Ok("Book has been delete from your wish list");
        }

        return NotFound();
    }
}