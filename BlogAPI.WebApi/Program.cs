using BlogApi.Application.Interfaces;
using BlogApi.Application.Mappers;
using BlogApi.Application.UseCases;
using BlogApi.Domain.Interfaces.Repositories;
using BlogApi.Domain.Interfaces.Services;
using BlogApi.Domain.Services;
using BlogAPI.Infrastructure.Data.Context;
using BlogAPI.Infrastructure.Data.Repositories;
using BlogAPI.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Blog API",
        Version = "v1"
    });
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IArticleRepository, ArticleRepository>();

builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<IArticleUseCase, ArticleUseCase>();

builder.Services.AddSingleton<RabbitMQService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Inicia o serviço de consumo de mensagens para o e-mail se necessário
// Você pode descomentar isso se quiser processar e-mails no mesmo serviço
// var rabbitMQService = app.Services.GetRequiredService<RabbitMQService>();
// rabbitMQService.ConsumeMessages<object>(msg => 
// {
//     // Lógica de processamento de e-mails aqui
//     // Em produção, você provavelmente teria um serviço separado para isso
// });

app.Run();