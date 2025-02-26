using Microsoft.OpenApi.Models;
using BlogApi.Application.Interfaces;
using BlogApi.Application.Mappers;
using BlogApi.Application.UseCases;
using BlogApi.Domain.Interfaces.Repositories;
using BlogApi.Domain.Interfaces.Services;
using BlogApi.Domain.Services;
using BlogAPI.Infrastructure.Data.Context;
using BlogAPI.Infrastructure.Data.Repositories;
using BlogAPI.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BlogAPI", Version = "v1" });
    
    // Configuração para utilizar o JWT no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configuração do DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Registrar Repositories
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();

// Registrar Domain Services
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Registrar Application UseCases
builder.Services.AddScoped<IArticleUseCase, ArticleUseCase>();

builder.Services.AddSingleton<RabbitMQService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Adiciona autenticação e autorização ao pipeline
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