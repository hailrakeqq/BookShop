using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BookShop.API.Model.Entity;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace BookShop.API.Authorization;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    RefreshTokenDocument GenerateRefreshToken(string userId);
    RefreshTokenDocument GetRefreshTokenDocumentById(string id);
    void UpdateRefreshTokenByUserId(RefreshTokenDocument refreshTokenDocument,string id);
    Task CreateRefreshTokenDocumentAsync(RefreshTokenDocument refreshTokenDocument);
}

public sealed class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly ApplicationDbContext _context;
    private IMongoCollection<RefreshTokenDocument> _refreshTokens =>
        _context.MongoDatabase.GetCollection<RefreshTokenDocument>("RefreshTokens");
    
    public TokenService(IConfiguration config, ApplicationDbContext context)
    {
        _config = config;
        _context = context;
    }

    public string GenerateAccessToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id!),
            new Claim(ClaimTypes.Name, user.Username!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Role, user.Role!)
        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public RefreshTokenDocument GenerateRefreshToken(string id)
    {
        //TODO: придумать норм имя
        var user = _refreshTokens.Find(Builders<RefreshTokenDocument>.Filter.Eq("UserId", id)).FirstOrDefault();
        if (user != null)
        {
            user.RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            return user;
        }
        
        return null;
    }

    public RefreshTokenDocument GetRefreshTokenDocumentById(string id)
    {
        var refreshTokenDocument = 
            _refreshTokens.Find(Builders<RefreshTokenDocument>.Filter.Eq("UserId", id)).FirstOrDefault();
        return refreshTokenDocument != null ? refreshTokenDocument : null!;
    }

    public void UpdateRefreshTokenByUserId(RefreshTokenDocument refreshTokenDocument,string id)
    {
        var filter = Builders<RefreshTokenDocument>.Filter.Eq("UserId", id);
        var updateToken = Builders<RefreshTokenDocument>.Update.Set("RefreshToken", refreshTokenDocument.RefreshToken);
        var updateCreatedTime = Builders<RefreshTokenDocument>.Update.Set("TokenCreated", refreshTokenDocument.TokenCreated);
        var updateExpiresTime = Builders<RefreshTokenDocument>.Update.Set("TokenExpires", refreshTokenDocument.TokenExpires);
        
        _refreshTokens.UpdateOne(filter, updateToken);
        _refreshTokens.UpdateOne(filter, updateCreatedTime);
        _refreshTokens.UpdateOne(filter, updateExpiresTime);
    }

    public async Task CreateRefreshTokenDocumentAsync(RefreshTokenDocument refreshTokenDocument)
    {
       await _refreshTokens.InsertOneAsync(refreshTokenDocument);
    }
}