using System.Security.Cryptography;
using System.Text;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces.Repositories;
using BlogApi.Domain.Interfaces.Services;

namespace BlogApi.Domain.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> RegisterUserAsync(string name, string email, string password)
    {
        var existingUser = await _userRepository.GetByEmailAsync(email);
        if (existingUser != null)
            throw new Exception("Email já está em uso");

        var passwordHash = HashPassword(password);
        var user = new User(name, email, passwordHash);
            
        return await _userRepository.AddAsync(user);
    }

    public async Task<string> AuthenticateAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
            throw new Exception("Usuário não encontrado");

        var passwordHash = HashPassword(password);
        if (user.PasswordHash != passwordHash)
            throw new Exception("Senha incorreta");
        
        return "jwt_token_placeholder";
    }

    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }
}