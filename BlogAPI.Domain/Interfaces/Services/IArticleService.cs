using BlogApi.Domain.Entities;

namespace BlogApi.Domain.Interfaces.Services;

public interface IArticleService
{
    Task<Article> CreateArticleAsync(string title, string content, Guid authorId);
    Task<IEnumerable<Article>> GetAllArticlesAsync();
    Task<Article> GetArticleByIdAsync(Guid id);
}