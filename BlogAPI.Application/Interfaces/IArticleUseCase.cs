using BlogApi.Application.DTOs;

namespace BlogApi.Application.Interfaces;

public interface IArticleUseCase
{
    Task<ArticleDTO> CreateArticleAsync(CreateArticleDTO articleDTO, Guid userId);
    Task<IEnumerable<ArticleDTO>> GetAllArticlesAsync();
    Task<ArticleDTO> GetArticleByIdAsync(Guid id);
}