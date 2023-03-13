using BookShop.API.Model.Entity;
using BookShop.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
        var currentBook = _bookRepository.isBookExistWithCurrentTitleFindByItem(book);

        if (!currentBook)
        {
            _bookRepository.Create(book);
            return Ok("Book has been successfully add");
        }

        return Conflict("Book with these title already exist");
    }
    
    //TODO: maybe i should add id to route for change book data, anyway i must test it
    [HttpPut]
    [Route("ChangeBookData/{id:Guid}")]
    public IActionResult ChangeBookData([FromRoute] string id, [FromBody] Book book)
    {
        var currentBook = _bookRepository.GetItem(id);
        if (currentBook != null)
        {
            book.Id = id;
            _bookRepository.Update(book);
            return Ok("Book has been successfully update");
        }

        return NotFound();
    }

    //TODO: i think i should change logic (delete by id)
    [HttpDelete]
    [Route("DeleteBook/{id:Guid}")]
    public IActionResult DeleteBook([FromRoute] string id)
    {
        var currentBook = _bookRepository.GetItem(id);
        if (currentBook != null)
        {
            _bookRepository.Delete(id);
            return Ok("Book has been successfully delete");
        }

        return NotFound();
    }
}