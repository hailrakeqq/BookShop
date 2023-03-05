using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : Controller
{
    [HttpPost]
    [Route("SaveBookCover")]
    [Authorize]
    public IActionResult SaveBookCover(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Please select a file to upload");

        var fileName = Path.GetFileName(file.FileName);
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        return Ok("File uploaded successfully!");
    }
}