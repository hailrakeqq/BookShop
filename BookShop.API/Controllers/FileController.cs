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
    public IActionResult SaveBookCover(IFormCollection requestContent)
    {
        var file = requestContent.Files[0];
        
        if (file == null || file.Length == 0)
            return BadRequest("Please select a file to upload");
        
        var fileName = Path.GetFileName(requestContent.Files[0].FileName);
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "bookCover", $"{fileName}.jpg");
        
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }
        
        return Ok("File uploaded successfully!");
    }
}