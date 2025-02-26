using AutoMapper;
using BlogApi.Application.DTOs;
using BlogApi.Domain.Interfaces.Services;

namespace BlogApi.Application.UseCases;

public class ArticleUseCase
{
    private readonly IArticleService _articleService;
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;

    public ArticleUseCase(
        IArticleService articleService, 

        IEmailService emailService,
        IMapper mapper)
    {
        _articleService = articleService;
        _emailService = emailService;
        _mapper = mapper;
    }

    public async Task<ArticleDTO> CreateArticleAsync(CreateArticleDTO articleDTO, Guid userId)
    {
        var article = await _articleService.CreateArticleAsync(
            articleDTO.Title, 
            articleDTO.Content, 
            userId);
        
        await _emailService.SendArticleCreatedEmailAsync("teste", article.Title);

        return _mapper.Map<ArticleDTO>(article);
    }

    public async Task<IEnumerable<ArticleDTO>> GetAllArticlesAsync()
    {
        var articles = await _articleService.GetAllArticlesAsync();
        return _mapper.Map<IEnumerable<ArticleDTO>>(articles);
    }

    public async Task<ArticleDTO> GetArticleByIdAsync(Guid id)
    {
        var article = await _articleService.GetArticleByIdAsync(id);
        return _mapper.Map<ArticleDTO>(article);
    }
}