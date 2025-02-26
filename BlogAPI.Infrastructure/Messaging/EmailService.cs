using System.Text.Json;
using BlogApi.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace BlogAPI.Infrastructure.Messaging;

public class EmailService : IEmailService
{
    private readonly RabbitMQService _rabbitMQService;
    private readonly ILogger<EmailService> _logger;

    public EmailService(RabbitMQService rabbitMQService, ILogger<EmailService> logger)
    {
        _rabbitMQService = rabbitMQService;
        _logger = logger;
    }

    public Task SendArticleCreatedEmailAsync(string recipientEmail, string articleTitle)
    {
        var emailMessage = new
        {
            To = recipientEmail,
            Subject = "Novo artigo criado",
            Body = $"Seu artigo '{articleTitle}' foi criado com sucesso!"
        };

        _logger.LogInformation("Enviando mensagem para a fila: {Message}", 
            JsonSerializer.Serialize(emailMessage));

        _rabbitMQService.PublishMessage(emailMessage);

        return Task.CompletedTask;
    }
}