using AutoMapper;
using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Interfaces.Services;

namespace BlogApi.Application.UseCases;

public class AuthUseCase : IAuthUseCase
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public AuthUseCase(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    public async Task<AuthResponseDTO> LoginAsync(LoginDTO loginDTO)
    {
        var token = await _authService.AuthenticateAsync(loginDTO.Email, loginDTO.Password);
            
        // Na implementação real, você decodificaria o token para obter o ID do usuário
        // e buscaria os dados completos do usuário
            
        return new AuthResponseDTO
        {
            Token = token,
            User = new UserDTO
            {
                // Valores fictícios para demonstração
                Id = Guid.NewGuid(),
                Name = "Usuário",
                Email = loginDTO.Email
            }
        };
    }

    public async Task<UserDTO> RegisterAsync(RegisterUserDTO registerDTO)
    {
        var user = await _authService.RegisterUserAsync(
            registerDTO.Name,
            registerDTO.Email,
            registerDTO.Password);

        return _mapper.Map<UserDTO>(user);
    }
}