namespace BloggingApp.Models;

public class Post
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime PublishedOn { get; set; }

    public int CategoryId { get; set; }

    public Category? Category { get; set; }
}
