using MyApp.Common.DTOs.Auth;

namespace MyApp.Application;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);

    Task<AuthResponse> LoginAsync(LoginRequest request);
}