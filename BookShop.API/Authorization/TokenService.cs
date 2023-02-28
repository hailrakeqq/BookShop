using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookShop.API.Model.Entity;
using Microsoft.IdentityModel.Tokens;

namespace BookShop.API.Authorization;

public interface ITokenService
{
    string GenerateToken(User user);
}

public sealed class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(User user)
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
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}