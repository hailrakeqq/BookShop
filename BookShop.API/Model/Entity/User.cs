using System.ComponentModel.DataAnnotations;

namespace BookShop.API.Model.Entity;

public class User
{
    public string? Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; }
    public int CountOfProduct { get; set; } //if user seller
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