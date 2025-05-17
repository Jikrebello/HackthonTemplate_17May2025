using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyApp.Application.Interfaces.Services;
using MyApp.Common.Constants;
using MyApp.Common.DTOs.User;
using MyApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public UserService(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<UserResponse> CreateUserAsync(CreateUserRequest request)
    {
        var user = new AppUser
        {
            UserName = request.UserName,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = request.Role,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
        }

        // Ensure role exists
        if (!await _roleManager.RoleExistsAsync(request.Role))
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid>(request.Role));
        }

        // Add user to role
        await _userManager.AddToRoleAsync(user, request.Role);

        // Add permissions
        foreach (var permission in request.Permissions)
        {
            await AddPermissionAsync(user.Id, permission);
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
            .Include(u => u.Permissions)
            .ToListAsync();

        return await Task.WhenAll(users.Select(MapToResponseAsync));
    }

    public async Task<UserResponse?> GetUserByIdAsync(Guid id)
    {
        var user = await _userManager.Users
            .Include(u => u.Permissions)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return null;

        return await MapToResponseAsync(user);
    }

    public async Task<UserResponse?> GetUserByUsernameAsync(string username)
    {
        var user = await _userManager.Users
            .Include(u => u.Permissions)
            .FirstOrDefaultAsync(u => u.UserName == username);

        if (user == null)
            return null;

        return await MapToResponseAsync(user);
    }

    public async Task<UserResponse?> UpdateUserAsync(Guid id, UpdateUserRequest request)
    {
        var user = await _userManager.Users
            .Include(u => u.Permissions)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return null;

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        // Update role if it changed
        if (!string.IsNullOrEmpty(request.Role) && user.Role != request.Role)
        {
            // Remove from old role
            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);

            // Ensure new role exists
            if (!await _roleManager.RoleExistsAsync(request.Role))
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>(request.Role));
            }

            // Add to new role
            await _userManager.AddToRoleAsync(user, request.Role);
            user.Role = request.Role;
        }

        // Update permissions
        var currentPermissionEnums = user.Permissions
            .Where(p => Enum.TryParse<Permission>(p.PermissionName, out _))
            .Select(p => Enum.Parse<Permission>(p.PermissionName))
            .ToList();
            
        var permissionsToAdd = request.Permissions.Except(currentPermissionEnums).ToList();
        var permissionsToRemove = currentPermissionEnums.Except(request.Permissions).ToList();

        foreach (var permission in permissionsToAdd)
        {
            await AddPermissionAsync(user.Id, permission);
        }

        foreach (var permission in permissionsToRemove)
        {
            await RemovePermissionAsync(user.Id, permission);
        }

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

    public async Task<bool> AddPermissionAsync(Guid userId, Permission permission)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return false;

        // Check if permission already exists
        if (user.Permissions.Any(p => p.PermissionName == permission.ToString()))
            return true;

        user.Permissions.Add(new UserPermission
        {
            UserId = userId,
            PermissionName = permission.ToString()
        });

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<bool> RemovePermissionAsync(Guid userId, Permission permission)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return false;

        var permissionToRemove = user.Permissions
            .FirstOrDefault(p => p.PermissionName == permission.ToString());

        if (permissionToRemove == null)
            return true;

        user.Permissions.Remove(permissionToRemove);
        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    private async Task<UserResponse> MapToResponseAsync(AppUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        return new UserResponse
        {
            Id = user.Id,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = roles.FirstOrDefault() ?? user.Role,
            Permissions = user.Permissions
                .Where(p => Enum.TryParse<Permission>(p.PermissionName, out _))
                .Select(p => Enum.Parse<Permission>(p.PermissionName))
                .ToList()
        };
    }
}
