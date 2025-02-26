using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces.Repositories;
using BlogApi.Domain.Interfaces.Services;

namespace BlogApi.Domain.Services;

public class ArticleService : IArticleService
{
    private readonly IArticleRepository _articleRepository;

    public ArticleService(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }
    
    public async Task<Article> CreateArticleAsync(string title, string content, Guid authorId)
    {
        var article = new Article(title, content, authorId);
        return await _articleRepository.AddAsync(article);
    }

    public async Task<IEnumerable<Article>> GetAllArticlesAsync()
    {
        return await _articleRepository.GetAllAsync();
    }

    public async Task<Article> GetArticleByIdAsync(Guid id)
    {
        return await _articleRepository.GetByIdAsync(id);
    }
}