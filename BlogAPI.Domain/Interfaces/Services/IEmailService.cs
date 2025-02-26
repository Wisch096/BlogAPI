namespace BlogApi.Domain.Interfaces.Services;

public interface IEmailService
{
    Task SendArticleCreatedEmailAsync(string recipientEmail, string articleTitle);
}