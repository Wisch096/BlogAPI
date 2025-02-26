namespace BlogApi.Domain.Entities;

public class Article
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid AuthorId { get; set; }
    public User Author { get; set; }

    public Article(string title, string content, Guid authorId)
    {
        Id = Guid.NewGuid();
        Title = title;
        Content = content;
        AuthorId = authorId;
        CreatedAt = DateTime.UtcNow;
    }
    
    private Article() { }

    public void Update(string title, string content)
    {
        Title = title;
        Content = content;
    }
}