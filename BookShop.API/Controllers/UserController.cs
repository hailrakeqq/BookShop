using BookShop.API.Model.Entity;
using BookShop.API.Repository;
using BookShop.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BookShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly ISellerRepository _sellerRepository;
    private readonly IFileRepository _fileRepository;
    private readonly IBookRepository _bookRepository;
    public UserController(IUserRepository userRepository, 
                          ISellerRepository sellerRepository,
                          IFileRepository fileRepository,
                          IBookRepository bookRepository)
    {
        _userRepository = userRepository;
        _sellerRepository = sellerRepository;
        _fileRepository = fileRepository;
        _bookRepository = bookRepository;
    }

    [HttpGet]
    [Route("GetUserWishlist/{id:Guid}")]
    public IActionResult GetUserWishList([FromRoute] string id)
    {
        var wishlist = _userRepository.GetUserWishlist(id);
        if (wishlist != null)
            return Ok(wishlist);

        return NoContent();
    }

    [HttpGet]
    [Route("GetUserLibrary/{id:Guid}")]
    public IActionResult GetUserLibrary([FromRoute] string id)
    {
        var library = _userRepository.GetUserLibrary(id);
        if (library != null)
            return Ok(library);

        return NoContent();
    }

    [HttpPut]
    [Route("ChangeUserData/{id:Guid}")]
    public IActionResult ChangeUserData([FromRoute] string id, [FromBody] UserChangeDataModel newUserData)
    {
        var existingUser = _userRepository.GetItem(id);

        if (existingUser != null)
        {
            if (existingUser.Password == Toolchain.GenerateHash(newUserData.ConfirmPassword!))
            {
                if (!string.IsNullOrEmpty(newUserData.Username))
                    existingUser.Username = newUserData.Username;

                if (!string.IsNullOrEmpty(newUserData.Email))
                    existingUser.Email = newUserData.Email;

                if (!string.IsNullOrEmpty(newUserData.Password))
                    existingUser.Password = Toolchain.GenerateHash(newUserData.Password);

                _userRepository.Update(existingUser);

                return Ok("Your user data has been changed successfully.");
            }

            return Unauthorized("You entered the wrong password.");
        }

        return NotFound();
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public IActionResult DeleteUserAccount([FromRoute] string id)
    {
        var user = _userRepository.GetItem(id);
        if (user.Role == "seller")
        {
            _sellerRepository.DeleteSellerStats(id);
            
            var sellerBookCollection = _bookRepository.GetList().Where(b => b.SellerId == id).ToList();
            if (!sellerBookCollection.IsNullOrEmpty())
            {
                foreach (var book in sellerBookCollection)
                {
                    _fileRepository.DeleteFileByName("bookCover",book.Title!);
                    _bookRepository.Delete(book.Id!);
                }
            }
        }
        _userRepository.Delete(id);
        return Ok();
    }
}