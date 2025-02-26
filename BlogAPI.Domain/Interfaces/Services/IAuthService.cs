using BlogApi.Domain.Entities;

namespace BlogApi.Domain.Interfaces.Services;

public interface IAuthService
{
    Task<User> RegisterUserAsync(string name, string email, string password);
    Task<string> AuthenticateAsync(string email, string password);
}