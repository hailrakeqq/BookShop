using BookShop.API.Model.Entity;
using BookShop.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class SellerController : Controller
{
    private readonly IMongoBookRepository _bookRepository;

    public SellerController(IMongoBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    
    [HttpPost]
    [Route("AddBook")]
    public IActionResult AddBook([FromBody] Book book)
    {
        var currentBook = _bookRepository.CheckIfExist(book);

        if (!currentBook)
        {
            _bookRepository.Create(book);
            return Ok("Book has been successfully add");
        }

        return Conflict("Book with these title already exist");
    }

    [HttpPut]
    [Route("ChangeBookData")]
    public IActionResult ChangeBookData(Book book)
    {
        var currentBook = _bookRepository.CheckIfExist(book);
        if (currentBook)
        {
            _bookRepository.Update(book);
            return Ok("Book has been successfully update");
        }

        return NotFound();
    }

    [HttpDelete]
    [Route("DeleteBook/{id:Guid}")]
    public IActionResult DeleteBook(Book book)
    {
        var currentBook = _bookRepository.CheckIfExist(book);
        if (currentBook)
        {
            _bookRepository.Delete(book.Id!);
            return Ok("Book has been successfully delete");
        }

        return NotFound();
    }
}