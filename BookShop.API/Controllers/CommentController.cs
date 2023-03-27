using BookShop.API.Authorization;
using BookShop.API.Model.Entity;
using BookShop.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class CommentController : Controller
{
    private readonly ICommentRepository _commentRepository;

    public CommentController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }
    
    [HttpGet]
    [Route("GetCommentForBook/{id:Guid}")]
    [AllowAnonymous]
    public IActionResult GetCommentForBook([FromRoute] string id)
    {
        var commentCollection = _commentRepository.GetBookComment(id);
        return commentCollection != null ? Ok(commentCollection) : NotFound();
    }

    [HttpGet]
    [Route("GetUserComment/{id:Guid}")]    
    [AllowAnonymous]
    public IActionResult GetUserComment([FromRoute] string id)
    {
        var userComment = _commentRepository.GetUserComment(id);
        return userComment != null ? Ok(userComment) : NotFound();
    }

    [HttpPost]
    [Route("CreateBookComment/{id:Guid}")]
    public IActionResult CreateBookComment(CurrentUser currentUser, [FromBody] Comment comment, [FromRoute] string id)
    {
        comment.Id = Guid.NewGuid().ToString();
        comment.BookId = id;
        comment.UserId = currentUser.Id;
        comment.UserName = currentUser.Username;
        comment.TimeWhenCommentWasCreated = DateTime.Now;
        _commentRepository.Create(comment);
        return Ok(comment);
    }
    
    [HttpPut]
    [Route("UpdateBookComment/{id:Guid}")]
    public IActionResult UpdateBookComment(CurrentUser currentUser, [FromBody] Comment comment, [FromRoute] string id)
    {
        var existComment = _commentRepository.GetItem(id);
        if (existComment != null && _commentRepository.IsUserHaveCurrentComment(existComment, currentUser.Id))
        {
            _commentRepository.Update(comment);
            return Ok(comment);
        }

        return NotFound();
    }

    [HttpDelete]
    [Route("DeleteBookComment/{id:Guid}")]
    public IActionResult UpdateBookComment(CurrentUser currentUser, [FromRoute] string id)
    {
        var comment = _commentRepository.GetItem(id);
        if (comment != null && _commentRepository.IsUserHaveCurrentComment(comment, currentUser.Id))
        {
            _commentRepository.Delete(id);
            return Ok("Comment has been successfully deleted");
        }

        return NotFound();
    }
}