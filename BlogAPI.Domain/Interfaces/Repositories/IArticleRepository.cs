using BlogApi.Domain.Entities;

namespace BlogApi.Domain.Interfaces.Repositories;

public interface IArticleRepository
{
    Task<Article> GetByIdAsync(Guid id);
    Task<IEnumerable<Article>> GetAllAsync();
    Task<Article> AddAsync(Article article);
    Task UpdateAsync(Article article);
    Task DeleteAsync(Guid id);
}