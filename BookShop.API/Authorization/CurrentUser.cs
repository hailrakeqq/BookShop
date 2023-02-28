using System.Security.Claims;
using BookShop.API.Model.Entity;

namespace BookShop.API.Authorization;

// A scoped service that exposes the current user information
public class CurrentUser
{
    public User? User { get; set; }
    public ClaimsPrincipal Principal { get; set; } = default!;

    public string Id => Principal.FindFirstValue(ClaimTypes.NameIdentifier)!;
    public string Email => Principal.FindFirstValue(ClaimTypes.Email)!;
    public string Username => Principal.FindFirstValue(ClaimTypes.Name)!;
    public string Role => Principal.FindFirstValue(ClaimTypes.Role)!;
}