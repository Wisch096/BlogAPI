using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces.Repositories;
using BlogAPI.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Infrastructure.Data.Repositories;

public class ArticleRepository : IArticleRepository
{
    private readonly ApplicationDbContext _context;

    public ArticleRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Article> GetByIdAsync(Guid id)
    {
        return await _context.Articles
            .Include(a => a.Author)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Article>> GetAllAsync()
    {
        return await _context.Articles
            .Include(a => a.Author)
            .ToListAsync();
    }

    public async Task<Article> AddAsync(Article article)
    {
        await _context.Articles.AddAsync(article);
        await _context.SaveChangesAsync();
        return article;
    }

    public async Task UpdateAsync(Article article)
    {
        _context.Articles.Update(article);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var article = await _context.Articles.FindAsync(id);
        if (article != null)
        {
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
        }
    }
}