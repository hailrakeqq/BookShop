namespace BookShop.API.Model.Entity;

public class Comment
{
    public string? Id { get; set; }
    public string? BookId { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Text { get; set; }
    public DateTime TimeWhenCommentWasCreated { get; set; }
}