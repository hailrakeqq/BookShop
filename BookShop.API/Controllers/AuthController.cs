using BookShop.API.Authorization;
using BookShop.API.Model.Entity;
using BookShop.Tools;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace BookShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly LoginResponce _loginResponce;
    private readonly ITokenService _tokenService;

    public AuthController(ApplicationDbContext context, ITokenService tokenService, LoginResponce tokenResponce)
    {
        _context = context;
        _tokenService = tokenService;
        _loginResponce = tokenResponce;
    }

    private IMongoCollection<User> _users => _context.MongoDatabase.GetCollection<User>("Users");

    private IMongoCollection<UsersLibrary> _library =>
        _context.MongoDatabase.GetCollection<UsersLibrary>("UsersLibrary");

    private IMongoCollection<UsersWishlist> _wishList =>
        _context.MongoDatabase.GetCollection<UsersWishlist>("UsersWishlist");

    [HttpPost]
    [Route("Registration")]
    public async Task<IActionResult> CreateUser([FromBody]User user)
    {
        var currentUser = _users.Find(u => u.Email == user.Email!.ToLower() ||
                                           u.Username == user.Username!.ToLower()).FirstOrDefault();

        if (currentUser == null)
        {
            user.Email = user.Email!.ToLower();
            user.Username = user.Username!.ToLower();
            user.Id = Guid.NewGuid().ToString();
            user.Password = Toolchain.GenerateHash(user.Password!);

            var usersLibrary = new UsersLibrary();
            usersLibrary.Id = Guid.NewGuid().ToString();
            usersLibrary.OwnerId = user.Id;
            usersLibrary.BoughtBook = new HashSet<Book>();

            var usersWishList = new UsersWishlist();
            usersWishList.Id = Guid.NewGuid().ToString();
            usersWishList.OwnerId = user.Id;
            usersWishList.Wishlist = new HashSet<Book>();

            await _users.InsertOneAsync(user);
            await _library.InsertOneAsync(usersLibrary);
            await _wishList.InsertOneAsync(usersWishList);
            return Ok(user);
        }

        return Conflict("Account with these email or username already exist");
    }

    [HttpPost]
    [Route("Login")]
    public IActionResult Login([FromBody] UserLoginModel userLoginModel)
    {
        var currentUser = _users.Find(u => (u.Email == userLoginModel.EmailOrLogin!.ToLower() || 
                                            u.Username == userLoginModel.EmailOrLogin!.ToLower()) &&
                                            u.Password == Toolchain.GenerateHash(userLoginModel.Password))
            .FirstOrDefault();

        if (currentUser != null)
        {
            _loginResponce.Id = currentUser.Id;
            _loginResponce.Email = currentUser.Email;
            _loginResponce.Username = currentUser.Username;
            _loginResponce.Role = currentUser.Role;
            _loginResponce.JWTToken = _tokenService.GenerateToken(currentUser);
            _loginResponce.RefreshToken = "test";
            return Ok(_loginResponce);
        }

        return Unauthorized(currentUser);
    }
}