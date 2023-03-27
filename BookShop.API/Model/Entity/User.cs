using System.ComponentModel.DataAnnotations;
using MongoDB.Driver.Linq;

namespace BookShop.API.Model.Entity;

public class User
{
    public string? Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime TokenCreated { get; set; } = DateTime.Now;
    public DateTime TokenExpires { get; set; }  
    public string? Role { get; set; }
}

public class UserLoginModel
{
    [Required] public string? EmailOrLogin { get; set; }

    [Required] public string? Password { get; set; }
}

public class UserChangeDataModel
{
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; }

    [Required] public string? ConfirmPassword { get; set; }
}