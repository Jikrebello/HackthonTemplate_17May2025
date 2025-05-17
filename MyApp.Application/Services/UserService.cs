using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyApp.Application.Interfaces.Services;
using MyApp.Common.DTOs.User;
using MyApp.Domain.Entities;

namespace MyApp.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;

    public UserService(
        UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserResponse> CreateUserAsync(CreateUserRequest request)
    {
        var user = new AppUser
        {
            UserName = request.UserName,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
        }

        return await MapToResponseAsync(user);
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
            return false;

        var result = await _userManager.DeleteAsync(user);
        return result.Succeeded;
    }

    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
    {
        var users = await _userManager.Users
            .ToListAsync();

        return await Task.WhenAll(users.Select(MapToResponseAsync));
    }

    public async Task<UserResponse?> GetUserByIdAsync(Guid id)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return null;

        return await MapToResponseAsync(user);
    }

    public async Task<UserResponse?> GetUserByUsernameAsync(string username)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.UserName == username);

        if (user == null)
            return null;

        return await MapToResponseAsync(user);
    }

    public async Task<UserResponse?> UpdateUserAsync(Guid id, UpdateUserRequest request)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return null;

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        await _userManager.UpdateAsync(user);

        return await MapToResponseAsync(user);
    }

    public async Task<bool> UpdatePasswordAsync(Guid id, UpdatePasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
            return false;

        var result = await _userManager.ChangePasswordAsync(
            user, 
            request.CurrentPassword, 
            request.NewPassword);

        return result.Succeeded;
    }
    
    private async Task<UserResponse> MapToResponseAsync(AppUser user)
    {
        return new UserResponse
        {
            Id = user.Id,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }
}
