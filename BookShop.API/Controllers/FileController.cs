using BookShop.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : Controller
{
    private readonly IFileRepository _fileService;

    public FileController(IFileRepository fileService)
    {
        _fileService = fileService;
    }
    
    [HttpGet]
    [Route("GetAllBookCover")]
    public IActionResult SendAllBookCover()
    {
        var imagesArchive = _fileService.GetAllFileFromDirectory("bookCover", "bookCovers.zip");
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
        var requestedImage = _fileService.GetFileByName("bookCover", $"{BookName}.jpg");
        if(requestedImage != null)
            return new FileContentResult(requestedImage, "image/jpeg");
        
        var notFoundImage = _fileService.GetFileByName("bookCover", $"notfound.jpg");
        return new FileContentResult(notFoundImage, "image/jpeg");
    }

    [HttpPost]
    [Route("SaveBookCover")]
    [Authorize]
    public IActionResult SaveBookCover(IFormCollection requestContent)
    {
        if(_fileService.SaveFileToDirectory("bookCover", requestContent))
            return Ok("File uploaded successfully!");
        
        return BadRequest("Please select a file to upload");
    }
}