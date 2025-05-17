using MyApp.Common.DTOs.User;

namespace MyApp.Application.Interfaces.Services;

public interface IUserService
{
    Task<UserResponse> CreateUserAsync(CreateUserRequest request);
    Task<UserResponse?> GetUserByIdAsync(Guid id);
    Task<UserResponse?> GetUserByUsernameAsync(string username);
    Task<IEnumerable<UserResponse>> GetAllUsersAsync();
    Task<UserResponse?> UpdateUserAsync(Guid id, UpdateUserRequest request);
    Task<bool> UpdatePasswordAsync(Guid id, UpdatePasswordRequest request);
    Task<bool> DeleteUserAsync(Guid id);
}
