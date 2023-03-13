using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookShop.Tools;

namespace BookShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : Controller
{
    [HttpGet]
    [Route("GetAllBookCover")]
    public IActionResult SendAllBookCover()
    {
        Dictionary<string, byte[]> BookCoverCollection = new Dictionary<string, byte[]>();
        string[] filesCollection = Directory.GetFiles("bookCover", "*.jpg");

        foreach (var file in filesCollection)
        {
            var fileName = file.Split('/');
            BookCoverCollection.Add(fileName[1],System.IO.File.ReadAllBytes(file));
        }

        var imagesArchive = Toolchain.CreateZipArchiveFromFilesList(BookCoverCollection, "bookCovers.zip");
        return new FileContentResult(imagesArchive, "application/zip")
        {
            FileDownloadName = "bookCovers.zip"
        };
    }
    
    //TODO: if image with current name exist, replace it!(func for update)
    [HttpGet]
    [Route("GetBookCoverByName/{BookName}")]
    public IActionResult SendBookCoverByName([FromRoute] string BookName)
    {
        byte[] requestedImage = System.IO.File.ReadAllBytes($"bookCover/{BookName}.jpg");
        return new FileContentResult(requestedImage, "image/jpeg");
    }

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