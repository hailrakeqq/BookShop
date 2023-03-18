using System.Net;
using System.Net.Http.Headers;
using BookShop.API.Authorization;
using BookShop.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BookShop.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class BookController : Controller
{
    private readonly IBookRepository _bookRepository;
    private readonly IUserRepository _userRepository;

    public BookController(IBookRepository bookRepository, IUserRepository userRepository)
    {
        _bookRepository = bookRepository;
        _userRepository = userRepository;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetBookList()
    {
        return Ok(_bookRepository.GetList());
    }
    
    [HttpGet]
    [AllowAnonymous]
    [Route("GetSellerBookCollection/{id:Guid}")]
    public IActionResult GetCurrentSellerBookList([FromRoute] string id)
    {
        var bookCollection = _bookRepository.GetList().Where(b => b.SellerId == id);
        if(bookCollection.IsNullOrEmpty())
            return NoContent();
        return Ok(bookCollection);
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("GetBookById/{id:Guid}")]
    public IActionResult GetBookById([FromRoute] string id)
    {
        var book = _bookRepository.GetItem(id);
        return book != null ? Ok(book) : NotFound();
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