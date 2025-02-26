using System.Security.Claims;
using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticleController : ControllerBase
{
    private readonly IArticleUseCase _articleUseCase;

    public ArticleController(IArticleUseCase articleUseCase)
    {
        _articleUseCase = articleUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateArticleDTO articleDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _articleUseCase.CreateArticleAsync(articleDTO, userId);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var articles = await _articleUseCase.GetAllArticlesAsync();
        return Ok(articles);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        var article = await _articleUseCase.GetArticleByIdAsync(id);
        if (article == null)
            return NotFound();

        return Ok(article);
    }
}