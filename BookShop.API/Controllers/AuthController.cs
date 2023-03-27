using BookShop.API.Authorization;
using BookShop.API.Model.Entity;
using BookShop.API.Repository;
using BookShop.Tools;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace BookShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userService;
    
    public AuthController(ApplicationDbContext context, ITokenService tokenService, IUserRepository userService)
    {
        _context = context;
        _tokenService = tokenService;
        _userService = userService;
    }

    private IMongoCollection<User> _users => _context.MongoDatabase.GetCollection<User>("Users");
    private IMongoCollection<UsersLibrary> _library =>
        _context.MongoDatabase.GetCollection<UsersLibrary>("UsersLibrary");

    private IMongoCollection<UsersWishlist> _wishList =>
        _context.MongoDatabase.GetCollection<UsersWishlist>("UsersWishlist");

    private IMongoCollection<SellerStats> _sellerStats =>
        _context.MongoDatabase.GetCollection<SellerStats>("SellerStats");

    [HttpPost]
    [Route("Registration")]
    public async Task<IActionResult> CreateUser([FromBody]User user)
    {
        var currentUser = _users.Find(u => u.Email == user.Email!.ToLower() ||
                                           u.Username == user.Username!.ToLower()).FirstOrDefault();
        
        if (currentUser == null)
        {
            user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = user.Email!.ToLower(),
                Username = user.Username!.ToLower(),
                Password = Toolchain.GenerateHash(user.Password!),
                Role = user.Role
            };

            switch (user.Role)
            {
                case "seller":
                    var seller = user;
                    var sellerStats = new SellerStats
                    {
                        Id = Guid.NewGuid().ToString(),
                        SellerId = seller.Id,
                        CountOfProduct = 0,
                        CountOfSoldProduct = 0
                    };
                    await _users.InsertOneAsync(seller);
                    await _sellerStats.InsertOneAsync(sellerStats);
                    return Ok(seller);

                default:
                    var usersLibrary = new UsersLibrary
                    {
                        Id = Guid.NewGuid().ToString(),
                        OwnerId = user.Id,
                        BoughtBook = new HashSet<Book>()
                    };
                    var usersWishlist = new UsersWishlist
                    {
                        Id = Guid.NewGuid().ToString(),
                        OwnerId = user.Id,
                        Wishlist = new HashSet<Book>()
                    };
                    await _users.InsertOneAsync(user);
                    await _library.InsertOneAsync(usersLibrary);
                    await _wishList.InsertOneAsync(usersWishlist);
                    return Ok(user);
            }
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
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            var loginResponse = new LoginResponse
            {
                Id = currentUser.Id,
                Email = currentUser.Email,
                Username = currentUser.Username,
                Role = currentUser.Role,
                AccessToken = _tokenService.GenerateAccessToken(currentUser),
                RefreshToken = newRefreshToken.Token
            };
            _userService.UpdateRefreshTokenByUserId(newRefreshToken, currentUser.Id);
            return Ok(loginResponse);
        }

        return Unauthorized(currentUser);
    }

    [HttpPost("refresh-token")]
    public IActionResult RefreshToken([FromHeader]string UserId, [FromHeader] string refreshToken)
    {
        var user = _userService.GetItem(UserId);
        if(user.RefreshToken != refreshToken && user.TokenExpires < DateTime.Now)
            return Unauthorized("Invalid refresh token.");

        var newAccessToken = _tokenService.GenerateAccessToken(user);
        return Ok(newAccessToken);
    }
}