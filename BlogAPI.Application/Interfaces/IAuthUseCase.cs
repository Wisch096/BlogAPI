using BlogApi.Application.DTOs;

namespace BlogApi.Application.Interfaces;

public interface IAuthUseCase
{
    Task<AuthResponseDTO> LoginAsync(LoginDTO loginDTO);
    Task<UserDTO> RegisterAsync(RegisterUserDTO registerDTO);
}